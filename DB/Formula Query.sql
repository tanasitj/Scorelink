-- 1.D/E ratio
SELECT CONVERT(VARCHAR,FORMAT(A,'#,##')) Result
FROM (
SELECT CONVERT(DECIMAL,ISNULL(SumAmount1 / (SELECT SumAmount1 FROM SummaryScore WHERE CreateBy = '{0}' AND AccGroupId = 19),0),18) A 
FROM SummaryScore 
WHERE CreateBy = '{0}' AND AccGroupId = 18) TAB

-- 2.กำไร/ขาดทุนสุทธิ
SELECT '[Y1='+CONVERT(VARCHAR,FORMAT(A,'#,##'))+'] > [Y2='+CONVERT(VARCHAR,FORMAT(B,'#,##'))+'] > [Y3='+CONVERT(VARCHAR,FORMAT(C,'#,##'))+']' Result
FROM (
SELECT 
ISNULL(SumAmount1,0) A
,ISNULL(SumAmount2,0) B
,ISNULL(SumAmount3,0) C
FROM SummaryScore 
WHERE CreateBy = '{0}' AND AccGroupId = 3) TAB

-- 3.EBITDA
SELECT CONVERT(VARCHAR,FORMAT(ISNULL(A-B-C+D+E,''),'#,##')) Result
FROM (
SELECT ISNULL(SumAmount1,0) A,
ISNULL((SELECT SumAmount1 FROM SummaryScore WHERE CreateBy = '{0}' AND AccGroupId = 5),0) B,
ISNULL((SELECT SumAmount1 FROM SummaryScore WHERE CreateBy = '{0}' AND AccGroupId = 6),0) C,
ISNULL((SELECT SumAmount1 FROM SummaryScore WHERE CreateBy = '{0}' AND AccGroupId = 7),0) D,
ISNULL((SELECT SumAmount1 FROM SummaryScore WHERE CreateBy = '{0}' AND AccGroupId = 8),0) E
FROM SummaryScore 
WHERE CreateBy = '{0}' AND AccGroupId = 4) TAB

--  4.Current ratio
SELECT CONVERT(VARCHAR,FORMAT(A,'#,##')) Result
FROM (
SELECT CONVERT(DECIMAL,ISNULL(SumAmount1 / (SELECT SumAmount1 FROM SummaryScore WHERE CreateBy = '{0}' AND AccGroupId = 10),0),18) A 
FROM SummaryScore 
WHERE CreateBy = '{0}' AND AccGroupId = 9) TAB

-- 5.Quick ratio
SELECT CONVERT(VARCHAR,FORMAT((A - B) / C,'#,##')) Result
FROM (
SELECT CONVERT(INT,ISNULL(SumAmount1,0),18) A,
CONVERT(INT,ISNULL((SELECT SumAmount1 FROM SummaryScore WHERE CreateBy = '{0}' AND AccGroupId = 11),0),18) B,
CONVERT(INT,ISNULL((SELECT SumAmount1 FROM SummaryScore WHERE CreateBy = '{0}' AND AccGroupId = 10),0),18) C
FROM SummaryScore 
WHERE CreateBy = '{0}' AND AccGroupId = 9) TAB

-- 6.ต้นทุนขายต่อยอดขาย
SELECT CONVERT(VARCHAR,FORMAT(A,'#,##')) Result
FROM (
SELECT CONVERT(DECIMAL,ISNULL(SumAmount1 - (SELECT SumAmount1 FROM SummaryScore WHERE CreateBy = '1' AND AccGroupId = 13),0) / 100,18) A 
FROM SummaryScore 
WHERE CreateBy = '{0}' AND AccGroupId = 12) TAB

-- 7.ถ้ายอดขายลดลงเมื่อเทียบกับปีที่แล้ว
SELECT '[Y1='+CONVERT(VARCHAR,FORMAT(A,'#,##'))+'] > [Y2='+CONVERT(VARCHAR,FORMAT(A,'#,##'))+']' Result
FROM (
SELECT 
ISNULL(SumAmount1,0) A
,ISNULL(SumAmount2,0) B
FROM SummaryScore 
WHERE CreateBy = '{0}' AND AccGroupId = 13) TAB

-- 8.กระแสเงินสดสุทธิจากการดำเนินงาน [CFO]
SELECT '[CFO='+CONVERT(VARCHAR,FORMAT(A,'#,##'))+']' Result
FROM (
SELECT 
ISNULL(SumAmount1,0) A
FROM SummaryScore 
WHERE CreateBy = '{0}' AND AccGroupId = 14) TAB

-- 9.กระแสเงินสดสุทธิจากการลงทุน [CFI]
SELECT '[CFI='+CONVERT(VARCHAR,FORMAT(A,'#,##'))+']' Result
FROM (
SELECT 
ISNULL(SumAmount1,0) A
FROM SummaryScore 
WHERE CreateBy = '{0}' AND AccGroupId = 15) TAB

-- 10.กระแสเงินสดจากการจัดหาเงิน [CFF]
SELECT '[CFF='+CONVERT(VARCHAR,FORMAT(A,'#,##'))+']' Result
FROM (
SELECT 
ISNULL(SumAmount1,0) A
FROM SummaryScore 
WHERE CreateBy = '{0}' AND AccGroupId = 16) TAB