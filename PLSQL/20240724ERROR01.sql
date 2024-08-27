-- 실습 19-16 프로시저 오류 정보
CREATE OR REPLACE PROCEDURE pro_err
IS
    err_no      NUMBER;
BEGIN
    -- 고의적으로 콜론을 넣지 않고 오류를 유도하여보기
    err_no = 100;
    DBMS_OUTPUT.PUT_LINE(err_no);
END pro_err;
/
SHOW ERRORS;

-- ERROR를 USER_%에서 확인하기
SELECT * FROM USER_ERRORS
WHERE NAME = 'PRO_ERR';
