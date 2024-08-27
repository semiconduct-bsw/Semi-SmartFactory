--EMP테이블에서 부서번호가 30인 데이터만 호출해 봅시다.
SELECT * FROM EMP
WHERE DEPTNO = 30;
--직업이 매니저인 데이터만 호출
SELECT * FROM EMP
WHERE JOB = 'MANAGER';
