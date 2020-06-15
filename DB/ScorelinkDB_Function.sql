/*#***********************************************************************##
//#  Create by:    Marutdin < Rut >                                       ## 
//#  Create date:  12/06/2020                                             ## 
//#  Description:                                                         ##
//#  Change History                                                       ##
//#  ==================================================================   ##
//#  <Change_Date_Time>	<Change_By>		<Change_Description>              ##
//# ==================================================================    ##
//#	                                        Revise Code                   ##
//#**********************************************************************#*/
USE [Scorelink]
GO
/****** Object:  UserDefinedFunction [dbo].[F_DocumentDetail]    Script Date: 12/06/2563 11:38:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[F_DocumentDetail] (@DocID INT)
RETURNS TABLE
AS
RETURN
   select DISTINCT S.StatementId,D.DocId,S.StatementName,D.FootnoteNo,SUBSTRING(
(SELECT ',' + D.DocPageNo
FROM DocumentDetail as D
WHERE D.PageType = S.StatementId AND D.DocId = @DocID ORDER BY D.PageType
FOR XML PATH('')),2,1000) AS PageNo
FROM DocumentDetail as D
INNER JOIN StatementType as S ON D.PageType = S.StatementId 
WHERE D.DocId = @DocID
GROUP BY D.DocId,S.StatementId,S.StatementName,D.FootnoteNo