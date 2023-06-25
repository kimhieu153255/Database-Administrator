CONN SYSTEM/153255@localhost:1521/XEPDB1;
CREATE OR REPLACE PROCEDURE LOGIN_USER (
    p_username IN VARCHAR2,
    output OUT VARCHAR2
)
IS
    count_check_employee INTEGER;
    count_check_admin INTEGER;
    TEMP VARCHAR2(10);
BEGIN
    TEMP:= SUBSTR(p_username, INSTR(p_username, 'NV') + 2);
    SELECT COUNT(*) INTO count_check_employee FROM SYSTEM.Nhanvien WHERE MANV = TEMP;
    IF count_check_employee = 1 THEN 
        output := 'Employee';
        RETURN;
    END IF;
    
    SELECT COUNT(*) INTO count_check_admin FROM system.user_am  WHERE username = p_username;

    IF count_check_admin = 1 THEN 
        output := 'Admin';
        RETURN;
    END IF;
    
    output := 'Not a user';
END;
/
CREATE OR REPLACE PROCEDURE GRANT_LOGIN_EMPLOYEE
AS
    CURSOR CUR IS SELECT 'NV' || MANV FROM NHANVIEN WHERE ('NV' || MANV) IN (SELECT USERNAME FROM ALL_USERS);
    US VARCHAR2(10);
BEGIN
    OPEN CUR;
    LOOP
        FETCH CUR INTO US;
        EXIT WHEN CUR%NOTFOUND;
        EXECUTE IMMEDIATE 'GRANT EXECUTE ON SYSTEM.LOGIN_USER TO ' || US;
    END LOOP;
    CLOSE CUR;
END;
/
EXEC SYSTEM.GRANT_LOGIN_EMPLOYEE;
/
GRANT EXECUTE ON SYSTEM.LOGIN_USER TO AMDBtest;
/

