CONN SYSTEM/153255@localhost:1521/XEPDB1;

CREATE OR REPLACE PROCEDURE LOGIN_USER (
    p_username IN VARCHAR2,
    output1 OUT VARCHAR2,
    output2 OUT varchar2
)
IS
    count_check_employee INTEGER;
    count_check_admin INTEGER;
    TEMP VARCHAR2(10);
    role_user varchar(20);
BEGIN
    TEMP:= SUBSTR(p_username, INSTR(p_username, 'NV') + 2);
    SELECT COUNT(*) INTO count_check_employee FROM SYSTEM.Nhanvien WHERE MANV = TEMP;
    IF count_check_employee = 1 THEN 
        output1 := 'Employee';
        SELECT VAITRO INTO role_user FROM SYSTEM.Nhanvien WHERE MANV = TEMP;
        output2 := role_user;
        RETURN;
    END IF;
    
    SELECT COUNT(*) INTO count_check_admin FROM system.user_am  WHERE username = p_username;

    IF count_check_admin = 1 THEN 
        output1 := 'Admin';
        output2 := null;
        RETURN;
    END IF;
    
    output1 := 'Not a user';
    output2 := null;
END;
/
--EXEC SYSTEM.GRANT_LOGIN_EMPLOYEE;
/
GRANT EXECUTE ON SYSTEM.LOGIN_USER TO public;
/