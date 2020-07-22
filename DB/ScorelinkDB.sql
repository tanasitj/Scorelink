USE [Scorelink]
GO
ALTER TABLE [dbo].[OCRResult] DROP CONSTRAINT [FK_OCRResult_DocumentInfo]
GO
ALTER TABLE [dbo].[DocumentDetail] DROP CONSTRAINT [FK_DocumentDetail_DocumentInfo]
GO
ALTER TABLE [dbo].[DocumentArea] DROP CONSTRAINT [FK_DocumentArea_DocumentDetail]
GO
/****** Object:  Table [dbo].[User]    Script Date: 21/06/2020 20:53:48 ******/
DROP TABLE [dbo].[User]
GO
/****** Object:  Table [dbo].[SysConfig]    Script Date: 21/06/2020 20:53:48 ******/
DROP TABLE [dbo].[SysConfig]
GO
/****** Object:  Table [dbo].[OCRResult]    Script Date: 21/06/2020 20:53:48 ******/
DROP TABLE [dbo].[OCRResult]
GO
/****** Object:  Table [dbo].[DocumentInfo]    Script Date: 21/06/2020 20:53:48 ******/
DROP TABLE [dbo].[DocumentInfo]
GO
/****** Object:  Table [dbo].[DocumentArea]    Script Date: 21/06/2020 20:53:48 ******/
DROP TABLE [dbo].[DocumentArea]
GO
/****** Object:  Table [dbo].[AccTitleDict]    Script Date: 21/06/2020 20:53:48 ******/
DROP TABLE [dbo].[AccTitleDict]
GO
/****** Object:  UserDefinedFunction [dbo].[F_DocumentDetail]    Script Date: 21/06/2020 20:53:48 ******/
DROP FUNCTION [dbo].[F_DocumentDetail]
GO
/****** Object:  Table [dbo].[DocumentDetail]    Script Date: 21/06/2020 20:53:48 ******/
DROP TABLE [dbo].[DocumentDetail]
GO
/****** Object:  Table [dbo].[StatementType]    Script Date: 21/06/2020 20:53:48 ******/
DROP TABLE [dbo].[StatementType]
GO
/****** Object:  Table [dbo].[StatementType]    Script Date: 21/06/2020 20:53:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StatementType](
	[StatementId] [int] IDENTITY(1,1) NOT NULL,
	[StatementName] [varchar](50) NULL,
	[Active] [varchar](1) NULL,
	[CreateBy] [varchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateBy] [varchar](50) NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_StatementType] PRIMARY KEY CLUSTERED 
(
	[StatementId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentDetail]    Script Date: 21/06/2020 20:53:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentDetail](
	[DocDetId] [int] IDENTITY(1,1) NOT NULL,
	[DocId] [int] NOT NULL,
	[DocPageNo] [varchar](50) NOT NULL,
	[FootnoteNo] [varchar](50) NULL,
	[PageType] [varchar](2) NULL,
	[ScanStatus] [varchar](1) NULL,
	[PageFileName] [varchar](250) NULL,
	[PagePath] [varchar](500) NULL,
	[PageUrl] [varchar](500) NULL,
	[Selected] [varchar](1) NULL,
	[PatternNo] [varchar](2) NULL,
	[CreateBy] [varchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_DocumentDetail] PRIMARY KEY CLUSTERED 
(
	[DocDetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[F_DocumentDetail]    Script Date: 21/06/2020 20:53:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[F_DocumentDetail] (@DocID INT)
RETURNS TABLE
AS
RETURN
SELECT 
S.StatementId,
D.DocId,
S.StatementName,
D.FootnoteNo,
STUFF((SELECT ',' + D.DocPageNo FROM DocumentDetail as D WHERE D.PageType = S.StatementId AND D.DocId = @DocID ORDER BY LEN(D.DocPageNo),D.DocPageNo FOR XML PATH('')),1,1,'') AS PageNo
FROM DocumentDetail as D
INNER JOIN StatementType as S ON D.PageType = S.StatementId 
WHERE D.DocId = @DocID
GROUP BY D.DocId,S.StatementId,S.StatementName,D.FootnoteNo
GO
/****** Object:  Table [dbo].[AccTitleDict]    Script Date: 21/06/2020 20:53:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccTitleDict](
	[AccNo] [int] IDENTITY(1,1) NOT NULL,
	[AccountTitle] [varchar](500) NULL,
	[CreateBy] [varchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateBy] [varchar](50) NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_StandardizedAcc] PRIMARY KEY CLUSTERED 
(
	[AccNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentArea]    Script Date: 21/06/2020 20:53:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentArea](
	[AreaId] [int] IDENTITY(1,1) NOT NULL,
	[AreaNo] [int] NOT NULL,
	[DocId] [int] NOT NULL,
	[DocDetId] [int] NOT NULL,
	[DocPageNo] [varchar](50) NULL,
	[PageType] [varchar](2) NULL,
	[AreaX] [varchar](50) NULL,
	[AreaY] [varchar](50) NULL,
	[AreaH] [varchar](50) NULL,
	[AreaW] [varchar](50) NULL,
	[AreaPath] [varchar](500) NULL,
	[AreaUrl] [varchar](500) NULL,
	[CreateBy] [varchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_DocumentArea] PRIMARY KEY CLUSTERED 
(
	[AreaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentInfo]    Script Date: 21/06/2020 20:53:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentInfo](
	[DocId] [int] IDENTITY(1,1) NOT NULL,
	[FileUID] [varchar](50) NULL,
	[FileName] [varchar](250) NULL,
	[FilePath] [varchar](500) NULL,
	[FileUrl] [varchar](500) NULL,
	[CreateBy] [varchar](50) NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK_DocumentInfo] PRIMARY KEY CLUSTERED 
(
	[DocId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OCRResult]    Script Date: 21/06/2020 20:53:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OCRResult](
	[OCRId] [int] IDENTITY(1,1) NOT NULL,
	[DocId] [int] NOT NULL,
	[DocDetId] [int] NOT NULL,
	[RowNo] [varchar](50) NULL,
	[Footnote] [varchar](50) NULL,
	[AccountTitle] [varchar](500) NULL,
	[Amount] [varchar](50) NULL,
	[Modified] [varchar](1) NULL,
	[CreateBy] [varchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_OCRResult_1] PRIMARY KEY CLUSTERED 
(
	[OCRId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SysConfig]    Script Date: 21/06/2020 20:53:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysConfig](
	[ConstId] [int] IDENTITY(1,1) NOT NULL,
	[ConstName] [varchar](50) NULL,
	[ConstOutput] [varchar](50) NULL,
	[CreateBy] [varchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateBy] [varchar](50) NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_SysConstance] PRIMARY KEY CLUSTERED 
(
	[ConstId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 21/06/2020 20:53:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NULL,
	[Name] [varchar](50) NULL,
	[Surname] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
	[Company] [varchar](50) NULL,
	[Telephone] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
	[Admin] [varchar](50) NULL,
	[RegisterDate] [datetime] NULL,
	[ExpireDate] [datetime] NULL,
	[UpdateBy] [varchar](50) NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[StatementType] ON 

INSERT [dbo].[StatementType] ([StatementId], [StatementName], [Active], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate]) VALUES (1, N'Income Statement', N'1', N'1', CAST(N'2020-06-11T09:41:34.000' AS DateTime), N'1', CAST(N'2020-06-11T09:41:34.000' AS DateTime))
INSERT [dbo].[StatementType] ([StatementId], [StatementName], [Active], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate]) VALUES (2, N'Balance Sheet', N'1', N'1', CAST(N'2020-06-11T09:41:34.000' AS DateTime), N'1', CAST(N'2020-06-11T09:41:34.000' AS DateTime))
INSERT [dbo].[StatementType] ([StatementId], [StatementName], [Active], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate]) VALUES (3, N'Cash Flow', N'1', N'1', CAST(N'2020-06-11T09:41:34.000' AS DateTime), N'1', CAST(N'2020-06-11T09:41:34.000' AS DateTime))
INSERT [dbo].[StatementType] ([StatementId], [StatementName], [Active], [CreateBy], [CreateDate], [UpdateBy], [UpdateDate]) VALUES (4, N'Footnotes', N'1', N'1', CAST(N'2020-06-11T09:41:34.000' AS DateTime), N'1', CAST(N'2020-06-11T09:41:34.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[StatementType] OFF
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([UserId], [UserName], [Name], [Surname], [Password], [Email], [Company], [Telephone], [Status], [Admin], [RegisterDate], [ExpireDate], [UpdateBy], [UpdateDate]) VALUES (1, N'tanasitj', N'Tanasit', N'Jarutnuntipat', N'a', N'tanasit@mfec.co.th', N'MFEC', N'999999999', N'A', N'Y', NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[User] OFF
ALTER TABLE [dbo].[DocumentArea]  WITH CHECK ADD  CONSTRAINT [FK_DocumentArea_DocumentDetail] FOREIGN KEY([DocDetId])
REFERENCES [dbo].[DocumentDetail] ([DocDetId])
GO
ALTER TABLE [dbo].[DocumentArea] CHECK CONSTRAINT [FK_DocumentArea_DocumentDetail]
GO
ALTER TABLE [dbo].[DocumentDetail]  WITH CHECK ADD  CONSTRAINT [FK_DocumentDetail_DocumentInfo] FOREIGN KEY([DocId])
REFERENCES [dbo].[DocumentInfo] ([DocId])
GO
ALTER TABLE [dbo].[DocumentDetail] CHECK CONSTRAINT [FK_DocumentDetail_DocumentInfo]
GO
ALTER TABLE [dbo].[OCRResult]  WITH CHECK ADD  CONSTRAINT [FK_OCRResult_DocumentInfo] FOREIGN KEY([DocId])
REFERENCES [dbo].[DocumentInfo] ([DocId])
GO
ALTER TABLE [dbo].[OCRResult] CHECK CONSTRAINT [FK_OCRResult_DocumentInfo]
GO
