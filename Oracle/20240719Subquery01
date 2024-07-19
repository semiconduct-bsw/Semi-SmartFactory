-- 사원이름이 'JONES', 존스보다 급여를 많이 받는 사람은?
SELECT SAL FROM EMP WHERE ENAME = 'JONES';
-- 여기서 작성한 줄을 서브쿼리로 이용하여 동작시키기
SELECT * FROM EMP WHERE SAL > (SELECT SAL FROM EMP WHERE ENAME = 'JONES');

-- 서브쿼리를 사용하여 EMP 테이블의 사원 정보 중에서
-- 사원 이름 'ALLEN'인 사원의 COMM보다 많은 COMM을 받는 사원의이름을 모두 출력
SELECT ENAME FROM EMP WHERE COMM > (SELECT COMM FROM EMP WHERE ENAME = 'ALLEN')

-- EMP 테이블로 모든 직원들 중 급여가 평균보다 많이 받는 20번 부서의 직원 출력
SELECT E.EMPNO, E.ENAME, E.JOB, E.SAL, DEPTNO, D.DNAME, D.LOC
FROM EMP E NATURAL JOIN DEPT D
WHERE DEPTNO = 20 AND E.SAL > (SELECT ROUND(AVG(SAL),0) FROM EMP);
