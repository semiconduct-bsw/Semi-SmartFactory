-- 현재 날짜 SYSDATE
SELECT SYSDATE,
       SYSDATE - 1 AS "어제",
       SYSDATE + 1 AS "내일"
FROM DUAL;

-- 이후 몇 개월 후의 날짜
SELECT SYSDATE, ADD_MONTHS(SYSDATE, 2),
                ADD_MONTHS(SYSDATE, 120) AS "입사10년" FROM DUAL;
-- 마지막 날짜, 다음 날짜 구하기
SELECT SYSDATE,
    NEXT_DAY(SYSDATE, '월요일'),
    LAST_DAY(SYSDATE),
    LAST_DAY(TO_DATE('24/02/10'))
FROM DUAL;
