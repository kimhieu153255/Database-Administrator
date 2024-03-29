ALTER SESSION SET CONTAINER = XEPDB1;
ALTER SESSION SET "_ORACLE_SCRIPT" = TRUE;
/

GRANT SELECT ON SYS.DBA_ROLES TO SYSTEM WITH GRANT OPTION;
GRANT SELECT ON SYS.DBA_USERS TO SYSTEM WITH GRANT OPTION;
GRANT SELECT ON SYS.DBA_TABLES TO SYSTEM WITH GRANT OPTION;
GRANT SELECT ON SYS.DBA_VIEWS TO SYSTEM WITH GRANT OPTION;
GRANT SELECT ON SYS.DBA_TAB_PRIVS TO SYSTEM WITH GRANT OPTION;
GRANT SELECT ON SYS.DBA_ROLE_PRIVS TO SYSTEM WITH GRANT OPTION;
GRANT SELECT ON SYS.DBA_ROLE_PRIVS TO SYSTEM WITH GRANT OPTION;
GRANT EXECUTE ON DBMS_CRYPTO TO system;
grant all privileges to system;
/

connect system/catlonghd1604@localhost:1521/XEPDB1
/

DROP TABLE USER_AM ;
/

--Tao bang User
CREATE TABLE USER_AM (
    STT NUMBER GENERATED ALWAYS AS IDENTITY(START WITH 1 INCREMENT BY 1),
    FULLNAME VARCHAR2(20),
    USERNAME VARCHAR2(20) PRIMARY KEY,
    PASSWORD VARCHAR2(64),
    ROLE_NAME varchar2(200)
);
/

--DROP TABLE NHANVIEN CASCADE CONSTRAINTS;
--DROP TABLE DEAN CASCADE CONSTRAINTS;
--DROP TABLE PHONGBAN CASCADE CONSTRAINTS;
--DROP TABLE PHANCONG CASCADE CONSTRAINTS;
--
--
--CREATE TABLE NHANVIEN (
--    MANV VARCHAR2(10) PRIMARY KEY,
--    TENNV VARCHAR2(50) NOT NULL,
--    PHAI VARCHAR2(10) NOT NULL,
--    NGAYSINH DATE NOT NULL,
--    DIACHI VARCHAR2(100) NOT NULL,
--    SODT VARCHAR2(11) NOT NULL,
--    LUONG INT NOT NULL,
--    PHUCAP INT NOT NULL,
--    VAITRO VARCHAR2(20) NOT NULL,
--    MANQL VARCHAR2(10),
--    PHG VARCHAR2(10)
--);
--
--CREATE TABLE DEAN (
--    MADA VARCHAR2(10) PRIMARY KEY,
--    TENDA NVARCHAR2(50) NOT NULL,
--    NGAYBD DATE NOT NULL,
--    PHONG VARCHAR2(10) NOT NULL
--);
--
--CREATE TABLE PHONGBAN (
--    MAPB VARCHAR2(10) PRIMARY KEY,
--    TENPB VARCHAR2(50) NOT NULL,
--    TRPHG VARCHAR2(10) NOT NULL
--);
--
--CREATE TABLE PHANCONG (
--    MANV VARCHAR2(10),
--    MADA VARCHAR2(10),
--    THOIGIAN DATE NOT NULL,
--    CONSTRAINT PK_PHANCONG PRIMARY KEY (MANV, MADA)
--);
--
--ALTER TABLE NHANVIEN ADD CONSTRAINT FK_NHANVIEN_NHANVIEN FOREIGN KEY (MANQL) REFERENCES NHANVIEN (MANV);
--ALTER TABLE NHANVIEN ADD CONSTRAINT FK_NHANVIEN_PHONGBAN FOREIGN KEY (PHG) REFERENCES PHONGBAN (MAPB);
--ALTER TABLE PHANCONG ADD CONSTRAINT FK_PHANCONG_NHANVIEN FOREIGN KEY (MANV) REFERENCES NHANVIEN(MANV);
--ALTER TABLE PHANCONG ADD CONSTRAINT FK_PHANCONG_DEAN FOREIGN KEY (MADA) REFERENCES DEAN(MADA);
--ALTER TABLE PHONGBAN ADD CONSTRAINT FK_PHONGBAN_NHANVIEN FOREIGN KEY (TRPHG) REFERENCES NHANVIEN (MANV);
--ALTER TABLE DEAN ADD CONSTRAINT FK_DEAN_PHONG FOREIGN KEY (PHONG) REFERENCES PHONGBAN(MAPB);
--
--
--INSERT INTO NHANVIEN (MANV, TENNV, PHAI, NGAYSINH, DIACHI, SODT, LUONG, PHUCAP, VAITRO, MANQL)
--    WITH p AS(
--        SELECT '0001', 'Nguyen Van A', 'Nam', TO_DATE('01/01/1990', 'dd/mm/yyyy'), 'Ha Noi', '0987654321', 10000000, 2000000, 'Truong phong', NULL FROM dual UNION ALL
--        SELECT '0002', 'Nguyen Thi B', 'Nu', TO_DATE('02/02/1991', 'dd/mm/yyyy'), 'Ha Noi', '0912345678', 12000000, 2500000, 'Truong phong', NULL FROM dual UNION ALL
--        SELECT '0003', 'Tran Thi C', 'Nam', TO_DATE('03/03/1992', 'dd/mm/yyyy'), 'Ha Noi', '0987654321', 15000000, 3000000, 'QL truc tiep', NULL FROM dual UNION ALL
--        SELECT '0004', 'Le Van C', 'Nam', TO_DATE('02/01/1994', 'dd/mm/yyyy'), 'Ha Noi', '0441231231', 13000000,2000000, 'QL truc tiep', NULL FROM dual UNION ALL
--        SELECT '0005', 'Dao Thi D', 'Nu', TO_DATE('03/05/1993', 'dd/mm/yyyy'), 'Ha Noi', '0130123121', 4000000, 500000, 'Tai chinh', '0003' FROM dual UNION ALL
--        SELECT '0006', 'Pham Thi E', 'Nu', TO_DATE('04/03/1992', 'dd/mm/yyyy'), 'Ha Noi', '083123134', 8000000, 1000000, 'Tai chinh', '0003' FROM dual UNION ALL
--        SELECT '0007', 'Nguyen Van F', 'Nam', TO_DATE('05/12/1991', 'dd/mm/yyyy'), 'Ha Noi', '022532321', 10000000, 1500000, 'Nhan su', '0004' FROM dual UNION ALL
--        SELECT '0008', 'Tran Thi G', 'Nu', TO_DATE('06/09/1990', 'dd/mm/yyyy'), 'Ha Noi', '0234367821', 7000000, 900000, 'Nhan su', '0004' FROM dual UNION ALL
--        SELECT '0009', 'Phan Van H', 'Nam', TO_DATE('07/06/1989', 'dd/mm/yyyy'), 'Ha Noi', '0231234567', 5000000, 700000, 'Nhan vien', '0003' FROM dual UNION ALL
--        SELECT '0010', 'Ly Thi J', 'Nu', TO_DATE('08/04/1988', 'dd/mm/yyyy'), 'Ha Noi', '0224567890', 9000000, 1200000, 'Nhan vien', '0004' FROM dual UNION ALL
--        SELECT '0011', 'Cao Thi I', 'Nu', TO_DATE('10/04/1992', 'dd/mm/yyyy'), 'Ha Noi', '0224561120', 8000000, 1200000, 'Truong de an', '0003' FROM dual UNION ALL
--        SELECT '0012', 'Nguyen Thi K', 'Nu', TO_DATE('18/02/1988', 'dd/mm/yyyy'), 'Ha Noi', '0221267891', 9000000, 1200000, 'Truong de an', '0003' FROM dual
--) SELECT * FROM p;
--
--INSERT INTO PHONGBAN (MAPB, TENPB, TRPHG)
--    WITH p AS(
--        SELECT 'PB001', 'Phong kinh doanh', '0002' FROM dual UNION ALL
--        SELECT 'PB002', 'Phong nhan su', '0003' FROM dual
--) SELECT * FROM p;
--
--INSERT INTO DEAN (MADA, TENDA, NGAYBD, PHONG)
--    WITH p AS(
--        SELECT 'DA001', 'Du an ABC', TO_DATE('01/01/2022', 'dd/mm/yyyy'), 'PB001' FROM dual UNION ALL
--        SELECT 'DA002', 'Du an XYZ', TO_DATE('01/02/2022', 'dd/mm/yyyy'), 'PB001' FROM dual
--)SELECT * FROM p;
--
--INSERT INTO PHANCONG (MANV, MADA, THOIGIAN)
--    WITH p AS(
--        SELECT '0011', 'DA001', TO_DATE('08/05/2022', 'dd/mm/yyyy') FROM dual UNION ALL
--        SELECT '0008', 'DA001', TO_DATE('08/05/2022', 'dd/mm/yyyy') FROM dual UNION ALL
--        SELECT '0012', 'DA002', TO_DATE('02/06/2022', 'dd/mm/yyyy') FROM dual UNION ALL
--        SELECT '0009', 'DA002', TO_DATE('02/06/2022', 'dd/mm/yyyy') FROM dual
--) SELECT * FROM p;
--
--UPDATE NHANVIEN SET PHG='PB001' WHERE MANV='0001';
--UPDATE NHANVIEN SET PHG='PB002' WHERE MANV='0002';
--UPDATE NHANVIEN SET PHG='PB001' WHERE MANV='0003';
--UPDATE NHANVIEN SET PHG='PB002' WHERE MANV='0004';
--UPDATE NHANVIEN SET PHG='PB001' WHERE MANV='0005';
--UPDATE NHANVIEN SET PHG='PB001' WHERE MANV='0006';
--UPDATE NHANVIEN SET PHG='PB002' WHERE MANV='0007';
--UPDATE NHANVIEN SET PHG='PB002' WHERE MANV='0008';
--UPDATE NHANVIEN SET PHG='PB001' WHERE MANV='0009';
--UPDATE NHANVIEN SET PHG='PB002' WHERE MANV='0010';
--UPDATE NHANVIEN SET PHG='PB001' WHERE MANV='0011';
--UPDATE NHANVIEN SET PHG='PB001' WHERE MANV='0012';

--Trigger khi insert user
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
--Trigger
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


--Proc Login user
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

--Proc táº¡o user va gan quyen connect
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
    INSERT INTO SYSTEM.USER_AM(USERNAME,FULLNAME,PASSWORD)
    VALUES (USER,NAME,PASS);
    RESULT := 'SUCCESS';
END;
/

--Proc update password user
--Test
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

--Proc delete user
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

--Proc cap quyen tren bang user
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

--Thu hoi quyen tren bang cua user name hoac role
CREATE OR REPLACE PROCEDURE REVOKE_PRIVS_VIEW_USER_OR_ROLE(
    NAME IN VARCHAR2,
    PRIVS IN VARCHAR2,
    V_NAME IN VARCHAR2,
    RESULT OUT VARCHAR2)
AS
    COUNT_USER NUMBER := 0;
    COUNT_ROLE NUMBER := 0;
    COUNT_VIEW NUMBER := 0;
BEGIN
    SELECT COUNT(*) INTO COUNT_USER FROM DBA_USERS WHERE USERNAME = UPPER(NAME);
    SELECT COUNT(*) INTO COUNT_ROLE FROM DBA_ROLES WHERE ROLE = UPPER(NAME);
    SELECT COUNT(*) INTO COUNT_VIEW FROM DBA_VIEWS WHERE VIEW_NAME = UPPER(V_NAME);

    IF COUNT_USER = 0 AND COUNT_ROLE = 0 THEN
        RESULT := 'USER AND ROLE NOT EXISTS';
    ELSIF UPPER(PRIVS) NOT IN ('SELECT', 'INSERT', 'DELETE', 'UPDATE') THEN
        RESULT := 'PRIVS INVALID';
    ELSIF COUNT_VIEW = 0 THEN
        RESULT := 'VIEW NOT EXISTS';
    ELSE
    BEGIN
        EXECUTE IMMEDIATE ('REVOKE ' || PRIVS || ' ON SYSTEM.' || V_NAME || ' FROM ' || NAME);
        RESULT := 'SUCCESS';
    EXCEPTION
        WHEN OTHERS THEN
        BEGIN
            IF SQLCODE != -1031 THEN
                RAISE;
            ELSE
                RESULT := 'KHONG DU QUYEN';
            END IF;
        END;
    END;
    END IF;
END;
/


--Thu hoi quyen tren view cua user name hoac role
create or replace NONEDITIONABLE PROCEDURE REVOKE_PRIVS_TABLE_USER_OR_ROLE(
    NAME IN VARCHAR2,
    PRIVS IN VARCHAR2,
    TAB_NAME IN VARCHAR2,
    WITH_GRANT_OPTION IN INT,
    RESULT OUT VARCHAR2)
AS
    COUNT_USER NUMBER := 0;
    COUNT_ROLE NUMBER := 0;
    COUNT_TAB NUMBER := 0;
BEGIN
    SELECT COUNT(*) INTO COUNT_USER FROM DBA_USERS WHERE USERNAME = UPPER(NAME);
    SELECT COUNT(*) INTO COUNT_ROLE FROM DBA_ROLES WHERE ROLE = UPPER(NAME);
    SELECT COUNT(*) INTO COUNT_TAB FROM DBA_TABLES WHERE TABLE_NAME = UPPER(TAB_NAME);

    IF COUNT_USER = 0 AND COUNT_ROLE = 0 THEN
        RESULT := 'USER AND ROLE NOT EXISTS';
    ELSIF UPPER(PRIVS) NOT IN ('SELECT', 'INSERT', 'DELETE', 'UPDATE') THEN
        RESULT := 'PRIVS INVALID';
    ELSIF COUNT_TAB = 0 THEN
        RESULT := 'TABLE NOT EXISTS';
    ELSE
    BEGIN
        IF WITH_GRANT_OPTION = 1 AND COUNT_ROLE = 0 THEN
            EXECUTE IMMEDIATE ('REVOKE ' || PRIVS || ' ON SYSTEM.' || TAB_NAME || ' FROM ' || NAME || ' CASCADE CONSTRAINT');
            RESULT := 'SUCCESS';
        ELSIF WITH_GRANT_OPTION != 1 THEN
            EXECUTE IMMEDIATE ('REVOKE ' || PRIVS || ' ON SYSTEM.' || TAB_NAME || ' FROM ' || NAME);
            RESULT := 'SUCCESS';
        END IF;
    EXCEPTION
        WHEN OTHERS THEN
        BEGIN
            IF SQLCODE != -1031 THEN
                RAISE;
            ELSE
                RESULT := 'KHONG DU QUYEN';
            END IF;
        END;
    END;
    END IF;
END;

/

--Tao role
CREATE OR REPLACE PROCEDURE CREATE_ROLE(
    ROLE_NAME IN VARCHAR2,
    RESULT OUT VARCHAR2
)AS
    role_count NUMBER := 0;
    NAMEUPPER VARCHAR2(200);
BEGIN
    SELECT COUNT(*) INTO role_count FROM SYS.DBA_ROLES WHERE ROLE = ROLE_NAME;
    IF role_count > 0 THEN
        RESULT := 'ROLE ALREADY EXISTS';
    ELSE
        NAMEUPPER := UPPER(ROLE_NAME);
        EXECUTE IMMEDIATE ('CREATE ROLE '||NAMEUPPER);
        RESULT := 'SUCCESS CREATE ROLE';
    END IF;
END;
/

--Xoa role
CREATE OR REPLACE PROCEDURE DELETE_ROLE(
    ROLE_NAME IN VARCHAR2,
    RESULT OUT VARCHAR2
)AS
    role_count NUMBER;
BEGIN
    SELECT COUNT(*) INTO role_count FROM SYS.DBA_ROLES WHERE ROLE = ROLE_NAME;
    IF role_count = 0 THEN
        RESULT := 'ROLE DOES NOT EXIST';
    ELSE
        EXECUTE IMMEDIATE 'DROP ROLE ' || ROLE_NAME;
        RESULT := 'SUCCESS DELETE ROLE';
    END IF;
END;
/

--CAP QUYEN TREN TABLE CHO USER HOAC ROLE
CREATE OR REPLACE PROCEDURE GRANT_PRIVS_TAB_USER_OR_ROLE(
    NAME IN VARCHAR2,
    PRIVS IN VARCHAR2,
    TAB_NAME IN VARCHAR2,
    WITH_GRANT_OPTION IN INT,
    RESULT OUT VARCHAR2)
AS
    COUNT_USER NUMBER := 0;
    COUNT_ROLE NUMBER := 0;
    COUNT_TAB NUMBER := 0;
BEGIN
    SELECT COUNT(*) INTO COUNT_USER FROM DBA_USERS WHERE USERNAME = UPPER(NAME);
    SELECT COUNT(*) INTO COUNT_ROLE FROM DBA_ROLES WHERE ROLE = UPPER(NAME);
    SELECT COUNT(*) INTO COUNT_TAB FROM DBA_TABLES WHERE TABLE_NAME = UPPER(TAB_NAME);

    IF COUNT_USER = 0 AND COUNT_ROLE = 0 THEN
        RESULT := 'USER OR ROLE NOT EXISTS';
    ELSIF UPPER(PRIVS) NOT IN ('SELECT', 'INSERT', 'DELETE', 'UPDATE') THEN
        RESULT := 'PRIVS INVALID';
    ELSIF COUNT_TAB = 0 THEN
        RESULT := 'TABLE NOT EXISTS';
    ELSE
        BEGIN
            IF WITH_GRANT_OPTION = 1 AND COUNT_ROLE != 0 THEN
                RESULT := 'CANT GRANT "WITH GRANT OPTION" TO ROLE';
            ELSIF WITH_GRANT_OPTION = 1 AND COUNT_ROLE = 0 THEN
                EXECUTE IMMEDIATE ('GRANT ' || PRIVS || ' ON SYSTEM.' || TAB_NAME || ' TO ' || NAME || ' WITH GRANT OPTION');
                RESULT := 'SUCCESS';
            ELSE
                EXECUTE IMMEDIATE ('GRANT ' || PRIVS || ' ON SYSTEM.' || TAB_NAME || ' TO ' || NAME);
                RESULT := 'SUCCESS';
            END IF;
        EXCEPTION
            WHEN OTHERS THEN
            BEGIN
                IF SQLCODE != -1031 THEN
                    RAISE;
                ELSE
                    RESULT := 'KHONG DU QUYEN';
                END IF;
            END;
        END;
    END IF;
END;
/

--CAP QUYEN TREN VIEW CHO USER HOAC ROLE
CREATE OR REPLACE PROCEDURE GRANT_PRIVS_VIEW_USER_OR_ROLE(
    NAME IN VARCHAR2,
    PRIVS IN VARCHAR2,
    V_NAME IN VARCHAR2,
    WITH_GRANT_OPTION IN INT,
    RESULT OUT VARCHAR2)
AS
    COUNT_USER NUMBER := 0;
    COUNT_ROLE NUMBER := 0;
    COUNT_VIEW NUMBER := 0;
    OWNER_VIEW VARCHAR2(50);
BEGIN
    SELECT COUNT(*) INTO COUNT_USER FROM DBA_USERS WHERE USERNAME = UPPER(NAME);
    SELECT COUNT(*) INTO COUNT_ROLE FROM DBA_ROLES WHERE ROLE = UPPER(NAME);
    SELECT COUNT(*) INTO COUNT_VIEW FROM DBA_VIEWS WHERE VIEW_NAME = UPPER(V_NAME);

    IF COUNT_USER = 0 AND COUNT_ROLE = 0 THEN
        RESULT := 'USER AND ROLE NOT EXISTS';
    ELSIF UPPER(PRIVS) NOT IN ('SELECT', 'INSERT', 'DELETE', 'UPDATE') THEN
        RESULT := 'PRIVS INVALID';
    ELSIF COUNT_VIEW = 0 THEN
        RESULT := 'VIEW NOT EXISTS';
    ELSE
    BEGIN
        SELECT OWNER INTO OWNER_VIEW FROM DBA_VIEWS WHERE VIEW_NAME = UPPER(V_NAME);
        IF WITH_GRANT_OPTION = 1 THEN
            EXECUTE IMMEDIATE ('GRANT ' || PRIVS || ' ON ' || OWNER_VIEW || '.' || V_NAME || ' TO ' || NAME || ' WITH GRANT OPTION');
            RESULT := 'SUCCESS';
        ELSE
            EXECUTE IMMEDIATE ('GRANT ' || PRIVS || ' ON ' || OWNER_VIEW || '.' || V_NAME || ' TO ' || NAME);
            RESULT := 'SUCCESS';
        END IF;
    EXCEPTION
        WHEN OTHERS THEN
        BEGIN
            IF SQLCODE != -1031 THEN
                RAISE;
            ELSE
                RESULT := 'KHONG DU QUYEN';
            END IF;
        END;
    END;
    END IF;
END;
/

--GAN ROLE CHO USER
CREATE OR REPLACE PROCEDURE GRANT_ROLE_TO_USER(
    ROLE_NAME IN VARCHAR2,
    USER_NAME IN VARCHAR2,
    RESULT OUT VARCHAR2
    )
AS
    COUNT_ROLE NUMBER;
    COUNT_USER NUMBER;
BEGIN
    SELECT COUNT(*) INTO COUNT_USER FROM SYS.DBA_USERS WHERE USERNAME = UPPER(USER_NAME);
    SELECT COUNT(*) INTO COUNT_ROLE FROM SYS.DBA_ROLES WHERE ROLE = UPPER(ROLE_NAME);
    IF COUNT_ROLE = 0 THEN
        RESULT := 'ROLE NOT EXISTS TO GRANT';
    ELSIF COUNT_USER = 0 THEN
        RESULT := 'USER NOT EXISTS';
    ELSE
        BEGIN
            EXECUTE IMMEDIATE ('GRANT ' || ROLE_NAME || ' TO ' || USER_NAME);
            RESULT := 'SUCCESS';
        EXCEPTION
            WHEN OTHERS THEN
                RESULT := 'KHONG DU QUYEN';
        END;
    END IF;
END;
/

--THU HOI ROLE TU USER
CREATE OR REPLACE PROCEDURE REVOKE_ROLE_TO_USER(
    ROLE_NAME IN VARCHAR2,
    USER_NAME IN VARCHAR2,
    RESULT OUT VARCHAR2
    )
AS
    CHECK_ROLE_USER NUMBER;
BEGIN
    SELECT COUNT(*) INTO CHECK_ROLE_USER FROM SYS.DBA_ROLE_PRIVS WHERE GRANTEE = UPPER(USER_NAME) AND GRANTED_ROLE = UPPER(ROLE_NAME);
    IF CHECK_ROLE_USER = 0 THEN
        RESULT := 'ROLE NOT GRANTED TO USER';
    ELSE
        BEGIN
            EXECUTE IMMEDIATE ('REVOKE ' || ROLE_NAME || ' FROM ' || USER_NAME);
            RESULT := 'SUCCESS';
        EXCEPTION
            WHEN OTHERS THEN
                RESULT := 'NO SUCCESS';
        END;
    END IF;
END;
/

--XEM QUYEN CUA ROLE HOAC USER
CREATE OR REPLACE PROCEDURE SELECT_TAB_VIEW_PRIVS_USER_ROLE(
    O_NAME IN VARCHAR2,
    RESULT OUT VARCHAR2,
    CUR OUT SYS_REFCURSOR
)
AS
    COUNT_USER NUMBER;
    COUNT_ROLE NUMBER;
BEGIN
    SELECT COUNT(*) INTO COUNT_USER FROM DBA_USERS WHERE USERNAME = UPPER(O_NAME);
    SELECT COUNT(*) INTO COUNT_ROLE FROM DBA_ROLES WHERE ROLE = UPPER(O_NAME);
    IF COUNT_USER = 0 AND COUNT_ROLE = 0 THEN
        RESULT := 'USER AND ROLE NOT EXISTS';
    ELSE
        OPEN CUR FOR SELECT * FROM DBA_TAB_PRIVS WHERE GRANTEE = UPPER(O_NAME) AND (TYPE = 'TABLE' OR TYPE = 'VIEW');
    END IF;
END;
/

--INSERT INTO ROLEDB(ROLE_ID, NAME_PRIVILEGE, TABLE_EFFECTIVE) VALUES (1, 'ADMIN', 'USER_AM');
INSERT INTO SYSTEM.USER_AM(FULLNAME, USERNAME, PASSWORD, ROLE_NAME) VALUES ('ADMIN','AMDBtest','123', null);
/
DROP USER AMDBtest;
/
create user AMDBtest IDENTIFIED BY 123;
/
GRANT SELECT, INSERT, UPDATE, DELETE ON SYSTEM.USER_AM TO AMDBtest WITH GRANT OPTION;
GRANT CREATE USER TO AMDBtest;
GRANT SELECT, INSERT, UPDATE, DELETE ON SYSTEM.NHANVIEN TO AMDBtest WITH GRANT OPTION;
GRANT SELECT, INSERT, UPDATE, DELETE ON SYSTEM.DEAN TO AMDBtest WITH GRANT OPTION;
GRANT SELECT, INSERT, UPDATE, DELETE ON SYSTEM.PHONGBAN TO AMDBtest WITH GRANT OPTION;
GRANT SELECT, INSERT, UPDATE, DELETE ON SYSTEM.PHANCONG TO AMDBtest WITH GRANT OPTION;

GRANT DROP USER TO AMDBtest with admin option;
GRANT CREATE ROLE TO AMDBtest;
GRANT SELECT ON SYS.dba_sys_privs TO AMDBtest WITH GRANT OPTION;
GRANT SELECT ON SYS.DBA_ROLES TO AMDBtest WITH GRANT OPTION;
GRANT SELECT ON SYS.DBA_USERS TO AMDBtest WITH GRANT OPTION;
GRANT SELECT ON SYS.DBA_TAB_PRIVS TO AMDBtest WITH GRANT OPTION;
GRANT SELECT ON SYS.DBA_ROLE_PRIVS TO AMDBtest WITH GRANT OPTION;
GRANT SELECT ON SYS.DBA_ROLE_PRIVS TO AMDBtest WITH GRANT OPTION;


GRANT EXECUTE ANY PROCEDURE TO AMDBtest;
GRANT EXECUTE ON SYSTEM.USER_REGISTER TO AMDBtest;
GRANT EXECUTE ON SYSTEM.LOGIN_USER_DBA TO AMDBtest;
GRANT EXECUTE ON SYSTEM.UPDATE_USER TO AMDBtest;
GRANT EXECUTE ON SYSTEM.DELETE_USER TO AMDBtest;
GRANT EXECUTE ON SYSTEM.GRANT_PERMISSION TO AMDBtest;
--GRANT EXECUTE ON SYSTEM.REVOKE_PERMISSION_ROLE TO AMDBtest;
GRANT EXECUTE ON SYSTEM.CREATE_ROLE TO AMDBtest;
GRANT EXECUTE ON SYSTEM.GRANT_PRIVS_TAB_USER_OR_ROLE TO AMDBtest;
GRANT EXECUTE ON SYSTEM.REVOKE_ROLE_TO_USER TO AMDBtest;
/
GRANT CREATE SESSION TO AMDBtest WITH admin OPTION;


CONN SYSTEM/catlonghd1604@localhost:1521/XEPDB1;

select * from system.user_am