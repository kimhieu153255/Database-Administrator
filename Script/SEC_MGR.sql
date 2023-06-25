ALTER SESSION SET CONTAINER = CDB$ROOT;
alter user lbacsys identified by lbacsys account unlock container = all;
ALTER SESSION SET CONTAINER = XEPDB1;
SELECT VALUE FROM V$OPTION WHERE PARAMETER = 'Oracle Label Security';
SELECT STATUS FROM DBA_OLS_STATUS WHERE NAME = 'OLS_CONFIGURE_STATUS';
/
EXEC LBACSYS.CONFIGURE_OLS;
EXEC LBACSYS.OLS_ENFORCEMENT.ENABLE_OLS;
/
DECLARE
    N NUMBER;
BEGIN
    SELECT COUNT(*) INTO N FROM DBA_USERS WHERE USERNAME = 'SEC_MGR';
    EXECUTE IMMEDIATE 'ALTER SESSION SET "_ORACLE_SCRIPT"= TRUE';
    IF N != 0 THEN
        EXECUTE IMMEDIATE 'DROP USER SEC_MGR CASCADE';
    END IF;
    EXECUTE IMMEDIATE 'CREATE USER SEC_MGR IDENTIFIED BY 123';
    EXECUTE IMMEDIATE 'GRANT ALL PRIVILEGES TO SEC_MGR';
    EXECUTE IMMEDIATE 'GRANT SELECT ANY DICTIONARY TO SEC_MGR';
    EXECUTE IMMEDIATE 'ALTER SESSION SET "_ORACLE_SCRIPT"= FALSE';
END;
/

CONNECT LBACSYS/lbacsys@localhost:1521/XEPDB1;

GRANT EXECUTE ON SA_SYSDBA TO SEC_MGR;
GRANT EXECUTE ON SA_COMPONENTS TO SEC_MGR;
GRANT EXECUTE ON SA_LABEL_ADMIN TO SEC_MGR;
GRANT EXECUTE ON SA_USER_ADMIN TO SEC_MGR;
GRANT EXECUTE ON CHAR_TO_LABEL TO SEC_MGR;
GRANT EXECUTE ON SA_POLICY_ADMIN TO SEC_MGR;
GRANT LBAC_DBA TO SEC_MGR;


CONNECT SEC_MGR/123@localhost:1521/XEPDB1;

BEGIN
    EXECUTE IMMEDIATE 'DROP TABLE THONGBAO PURGE';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -942 THEN
            RAISE;
        END IF;
END;
/

CREATE TABLE THONGBAO (
    MATB NUMBER PRIMARY KEY,
    NOIDUNG VARCHAR2(1000)
    );
/

BEGIN
    EXECUTE IMMEDIATE 'DROP SEQUENCE AUTO_INCREMENT_SEQUENCE';
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -2289 THEN
            RAISE;
        END IF;
END;
/
CREATE SEQUENCE AUTO_INCREMENT_SEQUENCE START WITH 1 INCREMENT BY 1;
/

CREATE OR REPLACE TRIGGER AUTO_INCREMENT_TRIGGER
BEFORE INSERT ON THONGBAO
FOR EACH ROW
BEGIN
    SELECT AUTO_INCREMENT_SEQUENCE.NEXTVAL INTO :NEW.MATB
    FROM DUAL;
END;
/

BEGIN
    SA_SYSDBA.DROP_POLICY (
        policy_name => 'QLTB',
        drop_column => TRUE
        );
EXCEPTION
    WHEN OTHERS THEN
        IF SQLCODE != -12416 THEN
            RAISE;
        END IF;
END;
/

BEGIN
    SA_SYSDBA.CREATE_POLICY (
        policy_name => 'QLTB',
        column_name => 'LABEL_COL'
        );
END;
/

GRANT QLTB_DBA TO SEC_MGR;

CONNECT SEC_MGR/123@localhost:1521/XEPDB1;

BEGIN
    SA_COMPONENTS.CREATE_LEVEL (
        policy_name => 'QLTB',
        level_num => 3000,
        short_name => 'GD',
        long_name => 'GIAMDOC'
        );
    SA_COMPONENTS.CREATE_LEVEL (
        policy_name => 'QLTB',
        level_num => 2000,
        short_name => 'TP',
        long_name => 'TRUONGPHONG'
        );
    SA_COMPONENTS.CREATE_LEVEL (
        policy_name => 'QLTB',
        level_num => 1000,
        short_name => 'NV',
        long_name => 'NHANVIEN'
        );
END;
/

BEGIN
    SA_COMPONENTS.CREATE_COMPARTMENT (
        policy_name => 'QLTB',
        comp_num => 200,
        short_name => 'MB',
        long_name => 'MUABAN'
        );
    SA_COMPONENTS.CREATE_COMPARTMENT (
        policy_name => 'QLTB',
        comp_num => 400,
        short_name => 'SX',
        long_name => 'SANXUAT'
        );
    SA_COMPONENTS.CREATE_COMPARTMENT (
        policy_name => 'QLTB',
        comp_num => 600,
        short_name => 'GC',
        long_name => 'GIACONG'
        );
END;
/

BEGIN
    SA_COMPONENTS.CREATE_GROUP (
        policy_name => 'QLTB',
        group_num => 20,
        short_name => 'B',
        long_name => 'MIENBAC'
        );
    SA_COMPONENTS.CREATE_GROUP (
        policy_name => 'QLTB',
        group_num => 40,
        short_name => 'T',
        long_name => 'MIENTRUNG'
        );
    SA_COMPONENTS.CREATE_GROUP (
        policy_name => 'QLTB',
        group_num => 60,
        short_name => 'N',
        long_name => 'MIENNAM'
        );
END;
/

BEGIN
    SA_POLICY_ADMIN.APPLY_TABLE_POLICY (
        policy_name => 'QLTB',
        schema_name => 'SEC_MGR',
        table_name => 'THONGBAO',
        table_options => 'NO_CONTROL'
        );
END;
/
INSERT INTO SEC_MGR.THONGBAO VALUES (1, 'Thong bao den tong giam doc ca 3 mien', CHAR_TO_LABEL('QLTB', 'GD:MB,SX,GC:B,T,N'));
INSERT INTO SEC_MGR.THONGBAO VALUES (2, 'Thong bao den giam doc mien bac', CHAR_TO_LABEL('QLTB', 'GD:MB,SX,GC:B'));
INSERT INTO SEC_MGR.THONGBAO VALUES (3, 'Thong bao den giam doc mien trung', CHAR_TO_LABEL('QLTB', 'GD:MB,SX,GC:T'));
INSERT INTO SEC_MGR.THONGBAO VALUES (4, 'Thong bao den giam doc mien nam', CHAR_TO_LABEL('QLTB', 'GD:MB,SX,GC:N'));
INSERT INTO SEC_MGR.THONGBAO VALUES (5, 'Thong bao den truong phong mua ban mien bac', CHAR_TO_LABEL('QLTB', 'TP:MB:B'));
INSERT INTO SEC_MGR.THONGBAO VALUES (6, 'Thong bao den truong phong san xuat mien bac', CHAR_TO_LABEL('QLTB', 'TP:SX:B'));
INSERT INTO SEC_MGR.THONGBAO VALUES (7, 'Thong bao den truong phong gia cong mien bac', CHAR_TO_LABEL('QLTB', 'TP:GC:B'));
INSERT INTO SEC_MGR.THONGBAO VALUES (8, 'Thong bao den truong phong mua ban mien trung', CHAR_TO_LABEL('QLTB', 'TP:MB:T'));
INSERT INTO SEC_MGR.THONGBAO VALUES (9, 'Thong bao den truong phong san xuat mien trung', CHAR_TO_LABEL('QLTB', 'TP:SX:T'));
INSERT INTO SEC_MGR.THONGBAO VALUES (10, 'Thong bao den truong phong gia cong mien trung', CHAR_TO_LABEL('QLTB', 'TP:GC:T'));
INSERT INTO SEC_MGR.THONGBAO VALUES (11, 'Thong bao den truong phong mua ban mien nam', CHAR_TO_LABEL('QLTB', 'TP:MB:N'));
INSERT INTO SEC_MGR.THONGBAO VALUES (12, 'Thong bao den truong phong san xuat mien nam', CHAR_TO_LABEL('QLTB', 'TP:SX:N'));
INSERT INTO SEC_MGR.THONGBAO VALUES (13, 'Thong bao den truong phong gia cong mien nam', CHAR_TO_LABEL('QLTB', 'TP:GC:N'));
INSERT INTO SEC_MGR.THONGBAO VALUES (14, 'Thong bao den nhan vien mua ban mien bac', CHAR_TO_LABEL('QLTB', 'NV:MB:B'));
INSERT INTO SEC_MGR.THONGBAO VALUES (15, 'Thong bao den nhan vien san xuat mien bac', CHAR_TO_LABEL('QLTB', 'NV:SX:B'));
INSERT INTO SEC_MGR.THONGBAO VALUES (16, 'Thong bao den nhan vien gia cong mien bac', CHAR_TO_LABEL('QLTB', 'NV:GC:B'));
INSERT INTO SEC_MGR.THONGBAO VALUES (17, 'Thong bao den nhan vien mua ban mien trung', CHAR_TO_LABEL('QLTB', 'NV:MB:T'));
INSERT INTO SEC_MGR.THONGBAO VALUES (18, 'Thong bao den nhan vien san xuat mien trung', CHAR_TO_LABEL('QLTB', 'NV:SX:T'));
INSERT INTO SEC_MGR.THONGBAO VALUES (19, 'Thong bao den nhan vien gia cong mien trung', CHAR_TO_LABEL('QLTB', 'NV:GC:T'));
INSERT INTO SEC_MGR.THONGBAO VALUES (20, 'Thong bao den nhan vien mua ban mien nam', CHAR_TO_LABEL('QLTB', 'NV:MB:N'));
INSERT INTO SEC_MGR.THONGBAO VALUES (21, 'Thong bao den nhan vien san xuat mien nam', CHAR_TO_LABEL('QLTB', 'NV:SX:N'));
INSERT INTO SEC_MGR.THONGBAO VALUES (22, 'Thong bao den nhan vien gia cong mien nam', CHAR_TO_LABEL('QLTB', 'NV:GC:N'));
/
BEGIN
    SA_POLICY_ADMIN.REMOVE_TABLE_POLICY (
        policy_name => 'QLTB',
        schema_name => 'SEC_MGR',
        table_name => 'THONGBAO'
        );
END;
/
BEGIN
    SA_POLICY_ADMIN.APPLY_TABLE_POLICY (
        policy_name => 'QLTB',
        schema_name => 'SEC_MGR',
        table_name => 'THONGBAO',
        table_options => 'READ_CONTROL, WRITE_CONTROL'
        );
END;
/

CREATE OR REPLACE PROCEDURE CREATE_USER_OLS (USERNAME_IN IN VARCHAR2)
AS
    N NUMBER;
BEGIN
    SELECT COUNT(*) INTO N FROM DBA_USERS WHERE USERNAME = UPPER(USERNAME_IN);
    IF N != 0 THEN
        EXECUTE IMMEDIATE 'DROP USER ' || USERNAME_IN;
    END IF;
    EXECUTE IMMEDIATE 'CREATE USER ' || USERNAME_IN || ' IDENTIFIED BY 123';
    EXECUTE IMMEDIATE 'GRANT CREATE SESSION TO ' || USERNAME_IN;
    EXECUTE IMMEDIATE 'GRANT SELECT, INSERT, UPDATE ON SEC_MGR.THONGBAO TO ' || USERNAME_IN;
END;
/

BEGIN
    CREATE_USER_OLS('TONGGIAMDOC');
    CREATE_USER_OLS('TRUONGPHONG_SANXUAT_MIENNAM');
    CREATE_USER_OLS('GIAMDOC_CHINHANH_MIENBAC');
END;
/

BEGIN
    sa_user_admin.set_user_labels
		(policy_name    => 'QLTB',
		user_name       => 'TONGGIAMDOC',
		max_read_label  => 'GD:MB,SX,GC:B,T,N'
        );
    sa_user_admin.set_user_labels
		(policy_name    => 'QLTB',
		user_name       => 'TRUONGPHONG_SANXUAT_MIENNAM',
		max_read_label  => 'TP:SX:N'
        );
    sa_user_admin.set_user_labels
		(policy_name    => 'QLTB',
		user_name       => 'GIAMDOC_CHINHANH_MIENBAC',
		max_read_label  => 'GD:MB,SX,GC:B'
        );
END;
/

CONN TONGGIAMDOC/123;
select * from sec_mgr.thongbao;