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
SET IDENTITY_INSERT [dbo].[DocumentArea] ON 

INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (10, 1, 1047, 13, N'1', N'1', N'161', N'299', N'1136', N'252', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001\0001\0001.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001/0001/0001.tif', N'1', CAST(N'2020-06-21T04:42:47.000' AS DateTime), CAST(N'2020-06-21T04:42:47.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (11, 2, 1047, 13, N'1', N'1', N'419', N'299', N'1136', N'45', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001\0001\0002.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001/0001/0002.tif', N'1', CAST(N'2020-06-21T04:42:47.000' AS DateTime), CAST(N'2020-06-21T04:42:47.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (12, 1, 1047, 14, N'2', N'1', N'161', N'299', N'1136', N'252', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001\0002\0001.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001/0002/0001.tif', N'1', CAST(N'2020-06-21T04:42:51.000' AS DateTime), CAST(N'2020-06-21T04:42:51.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (13, 2, 1047, 14, N'2', N'1', N'419', N'299', N'1136', N'45', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001\0002\0002.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001/0002/0002.tif', N'1', CAST(N'2020-06-21T04:42:51.000' AS DateTime), CAST(N'2020-06-21T04:42:51.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (14, 1, 1047, 15, N'3', N'1', N'161', N'299', N'1136', N'252', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001\0003\0001.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001/0003/0001.tif', N'1', CAST(N'2020-06-21T04:42:53.000' AS DateTime), CAST(N'2020-06-21T04:42:53.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (15, 2, 1047, 15, N'3', N'1', N'419', N'299', N'1136', N'45', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001\0003\0002.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001/0003/0002.tif', N'1', CAST(N'2020-06-21T04:42:53.000' AS DateTime), CAST(N'2020-06-21T04:42:53.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (16, 1, 1047, 33, N'17', N'1', N'161', N'299', N'1136', N'252', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001\0017\0001.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001/0017/0001.tif', N'1', CAST(N'2020-06-21T04:42:56.000' AS DateTime), CAST(N'2020-06-21T04:42:56.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (17, 2, 1047, 33, N'17', N'1', N'419', N'299', N'1136', N'45', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001\0017\0002.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001/0017/0002.tif', N'1', CAST(N'2020-06-21T04:42:56.000' AS DateTime), CAST(N'2020-06-21T04:42:56.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (18, 1, 1048, 29, N'39', N'3', N'160', N'297', N'1128', N'250', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\283f603f-2c08-4323-952b-e72255a04641\00003\0039\0001.tif', N'http://localhost:52483/FileUploads/00000001/283f603f-2c08-4323-952b-e72255a04641/00003/0039/0001.tif', N'1', CAST(N'2020-06-21T03:36:03.000' AS DateTime), CAST(N'2020-06-21T03:36:03.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (19, 2, 1048, 29, N'39', N'3', N'416', N'297', N'1128', N'45', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\283f603f-2c08-4323-952b-e72255a04641\00003\0039\0002.tif', N'http://localhost:52483/FileUploads/00000001/283f603f-2c08-4323-952b-e72255a04641/00003/0039/0002.tif', N'1', CAST(N'2020-06-21T03:36:03.000' AS DateTime), CAST(N'2020-06-21T03:36:03.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (20, 3, 1048, 29, N'39', N'3', N'467', N'297', N'1128', N'160', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\283f603f-2c08-4323-952b-e72255a04641\00003\0039\0003.tif', N'http://localhost:52483/FileUploads/00000001/283f603f-2c08-4323-952b-e72255a04641/00003/0039/0003.tif', N'1', CAST(N'2020-06-21T03:36:03.000' AS DateTime), CAST(N'2020-06-21T03:36:03.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (21, 1, 1048, 30, N'40', N'3', N'160', N'297', N'1128', N'250', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\283f603f-2c08-4323-952b-e72255a04641\00003\0040\0001.tif', N'http://localhost:52483/FileUploads/00000001/283f603f-2c08-4323-952b-e72255a04641/00003/0040/0001.tif', N'1', CAST(N'2020-06-21T03:36:05.000' AS DateTime), CAST(N'2020-06-21T03:36:05.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (22, 2, 1048, 30, N'40', N'3', N'416', N'297', N'1128', N'45', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\283f603f-2c08-4323-952b-e72255a04641\00003\0040\0002.tif', N'http://localhost:52483/FileUploads/00000001/283f603f-2c08-4323-952b-e72255a04641/00003/0040/0002.tif', N'1', CAST(N'2020-06-21T03:36:05.000' AS DateTime), CAST(N'2020-06-21T03:36:05.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (23, 3, 1048, 30, N'40', N'3', N'467', N'297', N'1128', N'160', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\283f603f-2c08-4323-952b-e72255a04641\00003\0040\0003.tif', N'http://localhost:52483/FileUploads/00000001/283f603f-2c08-4323-952b-e72255a04641/00003/0040/0003.tif', N'1', CAST(N'2020-06-21T03:36:05.000' AS DateTime), CAST(N'2020-06-21T03:36:05.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (24, 1, 1048, 31, N'42', N'3', N'160', N'297', N'1128', N'250', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\283f603f-2c08-4323-952b-e72255a04641\00003\0042\0001.tif', N'http://localhost:52483/FileUploads/00000001/283f603f-2c08-4323-952b-e72255a04641/00003/0042/0001.tif', N'1', CAST(N'2020-06-21T03:36:07.000' AS DateTime), CAST(N'2020-06-21T03:36:07.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (25, 2, 1048, 31, N'42', N'3', N'416', N'297', N'1128', N'45', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\283f603f-2c08-4323-952b-e72255a04641\00003\0042\0002.tif', N'http://localhost:52483/FileUploads/00000001/283f603f-2c08-4323-952b-e72255a04641/00003/0042/0002.tif', N'1', CAST(N'2020-06-21T03:36:07.000' AS DateTime), CAST(N'2020-06-21T03:36:07.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (26, 3, 1048, 31, N'42', N'3', N'467', N'297', N'1128', N'160', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\283f603f-2c08-4323-952b-e72255a04641\00003\0042\0003.tif', N'http://localhost:52483/FileUploads/00000001/283f603f-2c08-4323-952b-e72255a04641/00003/0042/0003.tif', N'1', CAST(N'2020-06-21T03:36:07.000' AS DateTime), CAST(N'2020-06-21T03:36:07.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (27, 3, 1047, 13, N'1', N'1', N'471', N'299', N'1136', N'161', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001\0001\0003.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001/0001/0003.tif', N'1', CAST(N'2020-06-21T04:42:47.000' AS DateTime), CAST(N'2020-06-21T04:42:47.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (28, 3, 1047, 14, N'2', N'1', N'471', N'299', N'1136', N'161', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001\0002\0003.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001/0002/0003.tif', N'1', CAST(N'2020-06-21T04:42:51.000' AS DateTime), CAST(N'2020-06-21T04:42:51.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (29, 3, 1047, 15, N'3', N'1', N'471', N'299', N'1136', N'161', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001\0003\0003.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001/0003/0003.tif', N'1', CAST(N'2020-06-21T04:42:54.000' AS DateTime), CAST(N'2020-06-21T04:42:54.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (30, 3, 1047, 33, N'17', N'1', N'471', N'299', N'1136', N'161', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001\0017\0003.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001/0017/0003.tif', N'1', CAST(N'2020-06-21T04:42:56.000' AS DateTime), CAST(N'2020-06-21T04:42:56.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (31, 1, 1048, 22, N'6', N'1', N'160', N'297', N'1128', N'250', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\283f603f-2c08-4323-952b-e72255a04641\00001\0006\0001.tif', N'http://localhost:52483/FileUploads/00000001/283f603f-2c08-4323-952b-e72255a04641/00001/0006/0001.tif', N'1', CAST(N'2020-06-21T03:51:41.000' AS DateTime), CAST(N'2020-06-21T03:51:41.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (32, 2, 1048, 22, N'6', N'1', N'416', N'297', N'1128', N'45', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\283f603f-2c08-4323-952b-e72255a04641\00001\0006\0002.tif', N'http://localhost:52483/FileUploads/00000001/283f603f-2c08-4323-952b-e72255a04641/00001/0006/0002.tif', N'1', CAST(N'2020-06-21T03:51:41.000' AS DateTime), CAST(N'2020-06-21T03:51:41.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (33, 3, 1048, 22, N'6', N'1', N'467', N'297', N'1128', N'160', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\283f603f-2c08-4323-952b-e72255a04641\00001\0006\0003.tif', N'http://localhost:52483/FileUploads/00000001/283f603f-2c08-4323-952b-e72255a04641/00001/0006/0003.tif', N'1', CAST(N'2020-06-21T03:51:41.000' AS DateTime), CAST(N'2020-06-21T03:51:41.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (34, 1, 1047, 53, N'10', N'1', N'161', N'299', N'1136', N'252', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001\0010\0001.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001/0010/0001.tif', N'1', CAST(N'2020-06-21T04:42:58.000' AS DateTime), CAST(N'2020-06-21T04:42:58.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (35, 2, 1047, 53, N'10', N'1', N'419', N'299', N'1136', N'45', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001\0010\0002.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001/0010/0002.tif', N'1', CAST(N'2020-06-21T04:42:58.000' AS DateTime), CAST(N'2020-06-21T04:42:58.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (36, 3, 1047, 53, N'10', N'1', N'471', N'299', N'1136', N'161', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001\0010\0003.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001/0010/0003.tif', N'1', CAST(N'2020-06-21T04:42:58.000' AS DateTime), CAST(N'2020-06-21T04:42:58.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (37, 1, 1047, 54, N'11', N'1', N'161', N'299', N'1136', N'252', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001\0011\0001.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001/0011/0001.tif', N'1', CAST(N'2020-06-21T04:43:00.000' AS DateTime), CAST(N'2020-06-21T04:43:00.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (38, 2, 1047, 54, N'11', N'1', N'419', N'299', N'1136', N'45', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001\0011\0002.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001/0011/0002.tif', N'1', CAST(N'2020-06-21T04:43:00.000' AS DateTime), CAST(N'2020-06-21T04:43:00.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (39, 3, 1047, 54, N'11', N'1', N'471', N'299', N'1136', N'161', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001\0011\0003.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001/0011/0003.tif', N'1', CAST(N'2020-06-21T04:43:00.000' AS DateTime), CAST(N'2020-06-21T04:43:00.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (40, 1, 1047, 55, N'12', N'1', N'161', N'299', N'1136', N'252', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001\0012\0001.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001/0012/0001.tif', N'1', CAST(N'2020-06-21T04:43:02.000' AS DateTime), CAST(N'2020-06-21T04:43:02.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (41, 2, 1047, 55, N'12', N'1', N'419', N'299', N'1136', N'45', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001\0012\0002.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001/0012/0002.tif', N'1', CAST(N'2020-06-21T04:43:02.000' AS DateTime), CAST(N'2020-06-21T04:43:02.000' AS DateTime))
INSERT [dbo].[DocumentArea] ([AreaId], [AreaNo], [DocId], [DocDetId], [DocPageNo], [PageType], [AreaX], [AreaY], [AreaH], [AreaW], [AreaPath], [AreaUrl], [CreateBy], [CreateDate], [UpdateDate]) VALUES (42, 3, 1047, 55, N'12', N'1', N'471', N'299', N'1136', N'161', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001\0012\0003.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001/0012/0003.tif', N'1', CAST(N'2020-06-21T04:43:02.000' AS DateTime), CAST(N'2020-06-21T04:43:02.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[DocumentArea] OFF
SET IDENTITY_INSERT [dbo].[DocumentDetail] ON 

INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (13, 1047, N'1', NULL, N'1', N'Y', N'00001', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001.tif', NULL, N'1', N'1', CAST(N'2020-06-16T05:39:52.000' AS DateTime), CAST(N'2020-06-21T04:42:47.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (14, 1047, N'2', NULL, N'1', N'Y', N'00001', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001.tif', NULL, N'1', N'1', CAST(N'2020-06-16T05:40:30.000' AS DateTime), CAST(N'2020-06-21T04:42:51.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (15, 1047, N'3', NULL, N'1', N'Y', N'00001', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001.tif', NULL, N'1', N'1', CAST(N'2020-06-16T05:40:34.000' AS DateTime), CAST(N'2020-06-21T04:42:54.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (16, 1047, N'4', NULL, N'2', NULL, N'00002', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00002.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00002.tif', NULL, NULL, N'1', CAST(N'2020-06-16T05:40:40.000' AS DateTime), CAST(N'2020-06-18T09:45:48.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (17, 1047, N'5', NULL, N'2', NULL, N'00002', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00002.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00002.tif', NULL, NULL, N'1', CAST(N'2020-06-16T05:40:43.000' AS DateTime), CAST(N'2020-06-18T09:45:48.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (18, 1047, N'6', NULL, N'2', NULL, N'00002', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00002.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00002.tif', NULL, NULL, N'1', CAST(N'2020-06-16T05:40:46.000' AS DateTime), CAST(N'2020-06-18T09:45:48.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (19, 1047, N'7', NULL, N'3', NULL, N'00003', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00003.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00003.tif', NULL, NULL, N'1', CAST(N'2020-06-16T05:40:50.000' AS DateTime), CAST(N'2020-06-17T07:34:41.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (20, 1047, N'8', NULL, N'3', NULL, N'00003', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00003.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00003.tif', NULL, NULL, N'1', CAST(N'2020-06-16T05:40:54.000' AS DateTime), CAST(N'2020-06-17T07:34:41.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (21, 1047, N'9', NULL, N'3', NULL, N'00003', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00003.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00003.tif', NULL, NULL, N'1', CAST(N'2020-06-16T05:40:58.000' AS DateTime), CAST(N'2020-06-17T07:34:41.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (22, 1048, N'6', NULL, N'1', N'Y', N'00001', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\283f603f-2c08-4323-952b-e72255a04641\00001.tif', N'http://localhost:52483/FileUploads/00000001/283f603f-2c08-4323-952b-e72255a04641/00001.tif', NULL, N'1', N'1', CAST(N'2020-06-17T07:53:15.000' AS DateTime), CAST(N'2020-06-21T03:51:41.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (23, 1048, N'7', NULL, N'1', N'', N'00001', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\283f603f-2c08-4323-952b-e72255a04641\00001.tif', N'http://localhost:52483/FileUploads/00000001/283f603f-2c08-4323-952b-e72255a04641/00001.tif', NULL, N'1', N'1', CAST(N'2020-06-17T07:53:20.000' AS DateTime), CAST(N'2020-06-21T03:51:37.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (24, 1048, N'8', NULL, N'1', N'', N'00001', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\283f603f-2c08-4323-952b-e72255a04641\00001.tif', N'http://localhost:52483/FileUploads/00000001/283f603f-2c08-4323-952b-e72255a04641/00001.tif', NULL, N'1', N'1', CAST(N'2020-06-17T07:53:23.000' AS DateTime), CAST(N'2020-06-21T03:51:37.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (25, 1048, N'10', NULL, N'2', NULL, N'00002', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\283f603f-2c08-4323-952b-e72255a04641\00002.tif', N'http://localhost:52483/FileUploads/00000001/283f603f-2c08-4323-952b-e72255a04641/00002.tif', NULL, NULL, N'1', CAST(N'2020-06-17T07:53:28.000' AS DateTime), CAST(N'2020-06-17T08:07:29.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (26, 1048, N'26', NULL, N'2', NULL, N'00002', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\283f603f-2c08-4323-952b-e72255a04641\00002.tif', N'http://localhost:52483/FileUploads/00000001/283f603f-2c08-4323-952b-e72255a04641/00002.tif', NULL, NULL, N'1', CAST(N'2020-06-17T07:53:40.000' AS DateTime), CAST(N'2020-06-17T08:07:29.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (27, 1048, N'32', NULL, N'2', NULL, N'00002', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\283f603f-2c08-4323-952b-e72255a04641\00002.tif', N'http://localhost:52483/FileUploads/00000001/283f603f-2c08-4323-952b-e72255a04641/00002.tif', NULL, NULL, N'1', CAST(N'2020-06-17T07:53:44.000' AS DateTime), CAST(N'2020-06-17T08:07:29.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (29, 1048, N'39', NULL, N'3', N'', N'00003', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\283f603f-2c08-4323-952b-e72255a04641\00003.tif', N'http://localhost:52483/FileUploads/00000001/283f603f-2c08-4323-952b-e72255a04641/00003.tif', NULL, N'1', N'1', CAST(N'2020-06-17T07:53:57.000' AS DateTime), CAST(N'2020-06-21T03:56:00.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (30, 1048, N'40', NULL, N'3', N'', N'00003', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\283f603f-2c08-4323-952b-e72255a04641\00003.tif', N'http://localhost:52483/FileUploads/00000001/283f603f-2c08-4323-952b-e72255a04641/00003.tif', NULL, N'1', N'1', CAST(N'2020-06-17T07:54:04.000' AS DateTime), CAST(N'2020-06-21T03:56:00.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (31, 1048, N'42', NULL, N'3', N'', N'00003', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\283f603f-2c08-4323-952b-e72255a04641\00003.tif', N'http://localhost:52483/FileUploads/00000001/283f603f-2c08-4323-952b-e72255a04641/00003.tif', NULL, N'1', N'1', CAST(N'2020-06-17T07:54:07.000' AS DateTime), CAST(N'2020-06-21T03:56:00.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (33, 1047, N'17', NULL, N'1', N'Y', N'00001', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001.tif', NULL, N'1', N'1', CAST(N'2020-06-18T09:43:08.000' AS DateTime), CAST(N'2020-06-21T04:42:56.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (45, 1049, N'1', NULL, N'1', N'', N'00001', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\4ebdbb94-026d-4381-b45d-536b7a50004d\00001.tif', N'http://localhost:52483/FileUploads/00000001/4ebdbb94-026d-4381-b45d-536b7a50004d/00001.tif', NULL, N'1', N'1', CAST(N'2020-06-21T04:10:16.000' AS DateTime), CAST(N'2020-06-21T04:11:56.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (46, 1049, N'2', NULL, N'1', N'', N'00001', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\4ebdbb94-026d-4381-b45d-536b7a50004d\00001.tif', N'http://localhost:52483/FileUploads/00000001/4ebdbb94-026d-4381-b45d-536b7a50004d/00001.tif', NULL, N'1', N'1', CAST(N'2020-06-21T04:10:22.000' AS DateTime), CAST(N'2020-06-21T04:11:56.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (47, 1049, N'3', NULL, N'1', N'', N'00001', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\4ebdbb94-026d-4381-b45d-536b7a50004d\00001.tif', N'http://localhost:52483/FileUploads/00000001/4ebdbb94-026d-4381-b45d-536b7a50004d/00001.tif', NULL, N'1', N'1', CAST(N'2020-06-21T04:10:26.000' AS DateTime), CAST(N'2020-06-21T04:11:56.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (48, 1049, N'4', NULL, N'1', N'', N'00001', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\4ebdbb94-026d-4381-b45d-536b7a50004d\00001.tif', N'http://localhost:52483/FileUploads/00000001/4ebdbb94-026d-4381-b45d-536b7a50004d/00001.tif', NULL, N'1', N'1', CAST(N'2020-06-21T04:10:30.000' AS DateTime), CAST(N'2020-06-21T04:11:56.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (49, 1049, N'5', NULL, N'1', N'', N'00001', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\4ebdbb94-026d-4381-b45d-536b7a50004d\00001.tif', N'http://localhost:52483/FileUploads/00000001/4ebdbb94-026d-4381-b45d-536b7a50004d/00001.tif', NULL, N'1', N'1', CAST(N'2020-06-21T04:10:37.000' AS DateTime), CAST(N'2020-06-21T04:11:56.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (51, 1049, N'6', NULL, N'2', NULL, N'00002', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\4ebdbb94-026d-4381-b45d-536b7a50004d.pdf', N'http://localhost:52483/FileUploads/00000001/4ebdbb94-026d-4381-b45d-536b7a50004d.pdf', NULL, NULL, N'1', CAST(N'2020-06-21T04:11:05.000' AS DateTime), CAST(N'2020-06-21T04:11:05.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (52, 1049, N'7', NULL, N'3', NULL, N'00003', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\4ebdbb94-026d-4381-b45d-536b7a50004d.pdf', N'http://localhost:52483/FileUploads/00000001/4ebdbb94-026d-4381-b45d-536b7a50004d.pdf', NULL, NULL, N'1', CAST(N'2020-06-21T04:11:13.000' AS DateTime), CAST(N'2020-06-21T04:11:13.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (53, 1047, N'10', NULL, N'1', N'Y', N'00001', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001.tif', NULL, N'1', N'1', CAST(N'2020-06-21T04:14:07.000' AS DateTime), CAST(N'2020-06-21T04:42:58.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (54, 1047, N'11', NULL, N'1', N'Y', N'00001', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001.tif', NULL, N'1', N'1', CAST(N'2020-06-21T04:14:16.000' AS DateTime), CAST(N'2020-06-21T04:43:00.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (55, 1047, N'12', NULL, N'1', N'Y', N'00001', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001.tif', NULL, N'1', N'1', CAST(N'2020-06-21T04:14:50.000' AS DateTime), CAST(N'2020-06-21T04:43:02.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (56, 1047, N'13', NULL, N'1', N'', N'00001', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001.tif', NULL, N'1', N'1', CAST(N'2020-06-21T04:14:57.000' AS DateTime), CAST(N'2020-06-21T04:42:42.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (57, 1047, N'14', NULL, N'1', N'', N'00001', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001.tif', NULL, N'1', N'1', CAST(N'2020-06-21T04:15:04.000' AS DateTime), CAST(N'2020-06-21T04:42:42.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (58, 1047, N'15', NULL, N'1', N'', N'00001', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001.tif', NULL, N'1', N'1', CAST(N'2020-06-21T04:15:07.000' AS DateTime), CAST(N'2020-06-21T04:42:42.000' AS DateTime))
INSERT [dbo].[DocumentDetail] ([DocDetId], [DocId], [DocPageNo], [FootnoteNo], [PageType], [ScanStatus], [PageFileName], [PagePath], [PageUrl], [Selected], [PatternNo], [CreateBy], [CreateDate], [UpdateDate]) VALUES (59, 1047, N'16', NULL, N'1', N'', N'00001', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f\00001.tif', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f/00001.tif', NULL, N'1', N'1', CAST(N'2020-06-21T04:15:16.000' AS DateTime), CAST(N'2020-06-21T04:42:42.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[DocumentDetail] OFF
SET IDENTITY_INSERT [dbo].[DocumentInfo] ON 

INSERT [dbo].[DocumentInfo] ([DocId], [FileUID], [FileName], [FilePath], [FileUrl], [CreateBy], [CreateDate]) VALUES (1047, N'c06ac8c8-e26d-4b87-a86f-3e6d74578b3f', N'DooDayDream EN.pdf', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\c06ac8c8-e26d-4b87-a86f-3e6d74578b3f.pdf', N'http://localhost:52483/FileUploads/00000001/c06ac8c8-e26d-4b87-a86f-3e6d74578b3f.pdf', N'1', CAST(N'2020-06-11T09:41:34.000' AS DateTime))
INSERT [dbo].[DocumentInfo] ([DocId], [FileUID], [FileName], [FilePath], [FileUrl], [CreateBy], [CreateDate]) VALUES (1048, N'283f603f-2c08-4323-952b-e72255a04641', N'EasyBuy.pdf', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\283f603f-2c08-4323-952b-e72255a04641.pdf', N'http://localhost:52483/FileUploads/00000001/283f603f-2c08-4323-952b-e72255a04641.pdf', N'1', CAST(N'2020-06-15T05:18:06.000' AS DateTime))
INSERT [dbo].[DocumentInfo] ([DocId], [FileUID], [FileName], [FilePath], [FileUrl], [CreateBy], [CreateDate]) VALUES (1049, N'4ebdbb94-026d-4381-b45d-536b7a50004d', N'Copy.pdf', N'C:\Tanasit\MFEC\Score Link\SRC\Scorelink\Scorelink.web\FileUploads\00000001\4ebdbb94-026d-4381-b45d-536b7a50004d.pdf', N'http://localhost:52483/FileUploads/00000001/4ebdbb94-026d-4381-b45d-536b7a50004d.pdf', N'1', CAST(N'2020-06-18T10:29:48.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[DocumentInfo] OFF
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
