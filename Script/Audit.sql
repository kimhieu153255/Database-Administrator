ALTER SESSION SET CONTAINER = XEPDB1;
ALTER SESSION SET "_ORACLE_SCRIPT" = TRUE;
/

--Cau 4a

BEGIN
    DBMS_FGA.DROP_POLICY (
        OBJECT_SCHEMA => 'SYSTEM',
        OBJECT_NAME => 'PHANCONG',
        POLICY_NAME => 'AU_THOIGIAN_PHANCONG'
    );
END;
/
BEGIN
    DBMS_FGA.ADD_POLICY(
        object_schema => 'SYSTEM',
        object_name => 'PHANCONG',
        POLICY_NAME => 'AU_THOIGIAN_PHANCONG',
        AUDIT_COLUMN => 'THOIGIAN',
        HANDLER_SCHEMA => Null,
        HANDLER_MODULE => Null,
        STATEMENT_TYPES => 'UPDATE'
    );
end;
/
--Lenh xem cac ban ghi audit 4a
SELECT FGA_POLICY_NAME, DBUSERNAME, EVENT_TIMESTAMP, ACTION_NAME, OBJECT_SCHEMA, OBJECT_NAME, SQL_TEXT
FROM UNIFIED_AUDIT_TRAIL 
WHERE FGA_POLICY_NAME = 'AU_THOIGIAN_PHANCONG'
ORDER BY EVENT_TIMESTAMP DESC;
/



--Cau 4b
BEGIN
    DBMS_FGA.DROP_POLICY (
        OBJECT_SCHEMA => 'SYSTEM',
        OBJECT_NAME => 'NHANVIEN',
        POLICY_NAME => 'AU_LUONG_PHUCAP_KHAC'
    );
END;
/
BEGIN
    DBMS_FGA.ADD_POLICY(
        object_schema => 'SYSTEM',
        object_name => 'NHANVIEN',
        POLICY_NAME => 'AU_LUONG_PHUCAP_KHAC',
        AUDIT_COLUMN => 'LUONG, PHUCAP',
        HANDLER_SCHEMA => NULL,
        HANDLER_MODULE => NULL,
        statement_types => 'SELECT',
        AUDIT_CONDITION => q'[SYS_CONTEXT('USERENV', 'SESSION_USER') <> 'NV'|| MANV]'
    );
END;
/

--Lenh xem cac ban ghi audit 4b
SELECT FGA_POLICY_NAME, DBUSERNAME, EVENT_TIMESTAMP, ACTION_NAME, OBJECT_SCHEMA, OBJECT_NAME, SQL_TEXT
FROM UNIFIED_AUDIT_TRAIL 
WHERE FGA_POLICY_NAME = 'AU_LUONG_PHUCAP_KHAC'
ORDER BY EVENT_TIMESTAMP DESC;



--Cau 4c

BEGIN
    DBMS_FGA.DROP_POLICY (
        OBJECT_SCHEMA => 'SYSTEM',
        OBJECT_NAME => 'NHANVIEN',
        POLICY_NAME => 'AU_NOT_TAICHINH'
    );
END;
/
BEGIN
    DBMS_FGA.ADD_POLICY(
        object_schema => 'SYSTEM',
        object_name => 'NHANVIEN',
        POLICY_NAME => 'AU_NOT_TAICHINH',
        AUDIT_COLUMN => 'LUONG, PHUCAP',
        HANDLER_SCHEMA => Null,
        HANDLER_MODULE => Null,
        statement_types => 'UPDATE',
        AUDIT_CONDITION => q'[VAITRO != 'Tai chinh']'
    );
end;
/
--Lenh xem cac ban ghi audit 4c
SELECT FGA_POLICY_NAME, DBUSERNAME, EVENT_TIMESTAMP, ACTION_NAME, OBJECT_SCHEMA, OBJECT_NAME, SQL_TEXT
FROM UNIFIED_AUDIT_TRAIL 
WHERE FGA_POLICY_NAME = 'AU_NOT_TAICHINH'
ORDER BY EVENT_TIMESTAMP DESC;
/