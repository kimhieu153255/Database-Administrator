ALTER SESSION SET CONTAINER = XEPDB1;
ALTER SESSION SET "_ORACLE_SCRIPT" = TRUE;
/

--GRANT EXECUTE ON SYS.DBMS_CRYPTO TO SYSTEM;
--GRANT EXECUTE ON SYS.UTL_I18N TO SYSTEM;
--GRANT CREATE USER TO SYSTEM;
--GRANT CREATE SESSION TO SYSTEM WITH ADMIN OPTION;
--GRANT drop user TO SYSTEM WITH ADMIN OPTION;
--GRANT SELECT ON SYS.dba_sys_privs TO SYSTEM WITH GRANT OPTION;

grant all privileges to system;
/

CONN SYSTEM/153255@localhost:1521/XEPDB1;
--DROP USER KIMHIEU1;
/

DROP TABLE USER_AM ;
/


CREATE TABLE USER_AM (
    STT NUMBER GENERATED ALWAYS AS IDENTITY(START WITH 1 INCREMENT BY 1),

    FULLNAME VARCHAR2(20),
    USERNAME VARCHAR2(20) PRIMARY KEY,
    PASSWORD VARCHAR2(64),
    ROLE_NAME varchar2(200)
--    FOREIGN KEY (ROLE_ID) REFERENCES ROLEDB(ROLE_ID)
);
/
CREATE OR REPLACE TRIGGER INSERT_USER_DBA
BEFORE INSERT OR UPDATE ON system.USER_AM
FOR EACH ROW
DECLARE
    PASSWORD_HASH VARCHAR2(64);
BEGIN
    PASSWORD_HASH := LOWER(RAWTOHEX(DBMS_CRYPTO.HASH(UTL_I18N.STRING_TO_RAW(:NEW.PASSWORD, 'AL32UTF8'), DBMS_CRYPTO.HASH_SH256)));
    :NEW.PASSWORD := PASSWORD_HASH;
    :NEW.USERNAME := UPPER(:NEW.USERNAME);

END;
/
CREATE OR REPLACE TRIGGER TRG_BEFORE_USER_AM
BEFORE INSERT ON SYSTEM.USER_AM
FOR EACH ROW
DECLARE
    SQLSTR VARCHAR2(200);
    TEMP VARCHAR2(200);
BEGIN
    DECLARE
        CUR1 SYS_REFCURSOR;
    BEGIN
        OPEN CUR1 FOR 'SELECT PRIVILEGE FROM SYS.dba_sys_privs WHERE grantee = :1' USING UPPER(:NEW.USERNAME);
        LOOP
            FETCH CUR1 INTO TEMP;
            EXIT WHEN CUR1%NOTFOUND;
            SQLSTR := SQLSTR || TEMP || ',';
        END LOOP;
        CLOSE CUR1;
    END;
    :NEW.ROLE_NAME := SQLSTR;
END;
/

CREATE OR REPLACE PROCEDURE LOGIN_USER_DBA(USER IN VARCHAR2, PASS IN VARCHAR2, RESULT OUT BOOLEAN)
AS
    PASSWORD_HASH VARCHAR2(64) := '';
    CURSOR CUR IS SELECT PASSWORD FROM system.USER_AM where USERNAME = USER;
BEGIN
    for c in cur loop
        PASSWORD_HASH := c.PASSWORD;
    end loop;
    IF PASSWORD_HASH = LOWER(RAWTOHEX(DBMS_CRYPTO.hash(UTL_I18N.STRING_TO_RAW(PASS, 'AL32UTF8'), DBMS_CRYPTO.HASH_SH256)))
    THEN
        RESULT := TRUE;
    ELSE
        RESULT := FALSE;
    END IF;
END;
/

/
CREATE OR REPLACE PROCEDURE USER_REGISTER(NAME IN VARCHAR2, USER IN VARCHAR2, PASS IN VARCHAR2, RESULT OUT VARCHAR2)
AS
    SQLSTR VARCHAR2(200);
    C NUMBER;
BEGIN
    SELECT COUNT(*) INTO C FROM SYSTEM.USER_AM WHERE USERNAME = USER;

    IF C>0 THEN
        RESULT := 'USER ALREADY EXISTS';
        return;
    END IF;

    EXECUTE IMMEDIATE ('CREATE USER '||USER||' IDENTIFIED BY '|| PASS);
    EXECUTE IMMEDIATE ('GRANT CREATE SESSION TO '|| USER);
    INSERT INTO SYSTEM.USER_AM(FULLNAME,USERNAME,PASSWORD)
    VALUES (NAME,USER,PASS);
    RESULT := 'SUCCESS';
END;
/

CREATE OR REPLACE PROCEDURE UPDATE_USER(
    FULL IN VARCHAR2,
    USER IN VARCHAR2,
    PASS IN VARCHAR2,
    RESULT OUT VARCHAR2
)
AS
    user_count NUMBER;
BEGIN
    SELECT COUNT(*) INTO user_count FROM SYSTEM.USER_AM WHERE USERNAME = USER;

    IF user_count > 0 THEN
        UPDATE USER_AM SET PASSWORD = PASS, FULLNAME = FULL WHERE USERNAME = USER;
        RESULT := 'User updated successfully';
    ELSE
       RESULT := 'User does not exist';
    END IF;
END;
/

CREATE OR REPLACE PROCEDURE DELETE_USER(USER_Name IN VARCHAR2, RESULT OUT VARCHAR2)
AS
    user_count NUMBER;
    strSQL VARCHAR2(200);
BEGIN
    SELECT COUNT(*) INTO user_count FROM SYSTEM.USER_AM WHERE USERNAME = UPPER( USER_Name);

    IF user_count > 0 THEN
        DELETE FROM SYSTEM.USER_AM WHERE USERNAME = upper(USER_Name);
        EXECUTE IMMEDIATE 'DROP USER ' || USER_Name;
        RESULT := 'User deleted successfully';
    ELSE
       RESULT := 'User does not exist';
    END IF;
END;
/

CREATE OR REPLACE PROCEDURE GRANT_PERMISSION (
    P_USERNAME IN VARCHAR2,
    P_PERMISSION IN VARCHAR2,
    RESULT OUT VARCHAR2
) AS
BEGIN
    IF p_permission NOT IN ('SELECT', 'INSERT', 'UPDATE','DELETE') THEN
        RESULT := 'Invalid';
    else
        IF P_PERMISSION = 'SELECT' THEN
            EXECUTE IMMEDIATE 'GRANT SELECT ON SYSTEM.USER_AM TO ' || P_USERNAME;
        ELSIF P_PERMISSION = 'INSERT' THEN
            EXECUTE IMMEDIATE 'GRANT SELECT, INSERT ON SYSTEM.USER_AM TO ' || P_USERNAME;
        ELSIF P_PERMISSION = 'UPDATE' THEN
            EXECUTE IMMEDIATE 'GRANT SELECT, UPDATE ON SYSTEM.USER_AM TO ' || P_USERNAME;
        ELSIF P_PERMISSION = 'DELETE' THEN
            EXECUTE IMMEDIATE 'GRANT DELETE ON SYSTEM.USER_AM TO ' || P_USERNAME;
        END IF;
        RESULT := 'SUCCESS';
    END IF;
END;
/

--INSERT INTO ROLEDB(ROLE_ID, NAME_PRIVILEGE, TABLE_EFFECTIVE) VALUES (1, 'ADMIN', 'USER_AM');
INSERT INTO SYSTEM.USER_AM(FULLNAME, USERNAME, PASSWORD, ROLE_NAME) VALUES ('ADMIN','AMDBtest','123', null);
/
DROP USER AMDBtest CASCADE;
/
create user AMDBtest IDENTIFIED BY 123;
/
GRANT SELECT, INSERT, UPDATE, DELETE ON SYSTEM.USER_AM TO AMDBtest WITH GRANT OPTION;
GRANT CREATE USER TO AMDBtest;
GRANT DROP USER TO AMDBtest with admin option;
GRANT SELECT ON SYS.dba_sys_privs TO AMDBtest WITH GRANT OPTION;
GRANT EXECUTE ANY PROCEDURE TO AMDBtest;
GRANT EXECUTE ON SYSTEM.USER_REGISTER TO AMDBtest;
GRANT EXECUTE ON SYSTEM.GET_PRIVILEGES_INTO_USER TO AMDBtest;
GRANT EXECUTE ON SYSTEM.LOGIN_USER_DBA TO AMDBtest;
GRANT EXECUTE ON SYSTEM.UPDATE_USER TO AMDBtest;
GRANT EXECUTE ON SYSTEM.DELETE_USER TO AMDBtest;
GRANT EXECUTE ON SYSTEM.GRANT_PERMISSION TO AMDBtest;
/
GRANT CREATE SESSION TO AMDBtest WITH admin OPTION;