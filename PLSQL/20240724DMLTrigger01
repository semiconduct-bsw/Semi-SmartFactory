-- 실습 19-31 EMP_TRG 테이블 생성
CREATE TABLE EMP_TRG AS SELECT * FROM EMP;
-- 실습 19-32 DML 실행 전에 수행할 트리거 생성
CREATE OR REPLACE TRIGGER trg_emp_nodm1_weekend
BEFORE
INSERT OR UPDATE OR DELETE ON EMP_TRG
BEGIN
    IF TO_CHAR(sysdate, 'DY') IN ('토', '일') THEN
        IF INSERTING THEN
            raise_application_error(-20000, '주말 사원정보 추가 불가');
        ELSIF UPDATING THEN
            raise_application_error(-20001, '주말 사원정보 수정 불가');
        ELSIF DELETING THEN
            raise_application_error(-20002, '주말 사원정보 삭제 불가');
        ELSE
            raise_application_error(-20003, '주말 사원정보 변경 불가');
        END IF;
    END IF;
END;
/

-- 평일 날짜 갱신, 주말 날짜 갱신
UPDATE emp_trg SET sal = 3500 WHERE EMPNO = 7839;
-- 만약 이걸 갱신하는 날짜가 토요일이거나 일요일 (PC기준)이라면 오류 발동
