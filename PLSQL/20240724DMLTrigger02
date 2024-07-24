-- 실습 19-35 EMP_TRG_LOG 테이블 생성 (AFTER)
CREATE TABLE EMP_TRG_LOG(
    TABLENAME VARCHAR2(10), -- DML이 수행된 테이블의 이름
    DML_TYPE  VARCHAR2(10), -- DML 명령어의 종류
    EMPNO     NUMBER(4),    -- DML 대상이 된 사원 번호
    USER_NAME VARCHAR2(30), -- DML을 수행한 USER 이름
    CHANGE_DATE DATE        -- DML이 수행된 날짜
);

-- 실습 19-36 DML 실행 후 수행할 트리거 생성
CREATE OR REPLACE TRIGGER trg_emp_log
-- EMP_TRG 테이블 내 데이터의 변경사항을 기록하는 트리거 생성
AFTER INSERT OR UPDATE OR DELETE ON EMP_TRG FOR EACH ROW

BEGIN
    IF INSERTING THEN
        INSERT INTO emp_trg_log
        VALUES ('EMP_TRG', 'INSERT', :new.empno,
                SYS_CONTEXT('USERENV', 'SESSION_USER'), sysdate);
    ELSIF UPDATING THEN
        INSERT INTO emp_trg_log
        VALUES ('EMP_TRG', 'UPDATE', :old.empno,
                SYS_CONTEXT('USERENV', 'SESSION_USER'), sysdate);
    ELSIF DELETING THEN
        INSERT INTO emp_trg_log
        VALUES ('EMP_TRG', 'DELETE', :old.empno,
                SYS_CONTEXT('USERENV', 'SESSION_USER'), sysdate);
    END IF;
END;
/

-- 실습 19-37~ EMP_TRG 테이블에 INSERT 실행
INSERT INTO EMP_TRG VALUES(9999, 'TESTEMP', 'CLERK', 7788, 
TO_DATE('2018-03-03','YYYY-MM-DD'),1200, null, 20);
COMMIT;

SELECT * FROM EMP_TRG;
-- EMP_TRG_LOG 확인
SELECT * FROM EMP_TRG_LOG;
-- EMP_TRG UPDATE 실행
UPDATE EMP_TRG SET SAL = 1300 WHERE MGR = 7788;
COMMIT;
