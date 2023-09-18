use master
drop Database LeVaTiShop
--Tao CSDL
create database LeVaTiShop
GO
use LeVaTiShop
GO
CREATE TABLE [dbo].[Users](
	[userID] [int] IDENTITY(1,1) NOT NULL,
	[userName] [varchar](20) NOT NULL,
	[password] [varchar](20) NOT NULL,
	[email] [varchar](20) NOT NULL,
	[fullName] [nvarchar](50) NOT NULL,
	[address] [nvarchar](100) NULL,
	[phone] [varchar](10) NOT NULL,
	[role] [bit] NOT NULL,
	[isDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[userID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_isDeleted]  DEFAULT ((0)) FOR [isDeleted]
GO

SET IDENTITY_INSERT [dbo].[Users] ON 

GO
INSERT [dbo].[Users] ([userID], [userName], [password], [email], [fullName], [address], [phone], [role], [isDeleted]) VALUES (1, N'admin', N'admin', N'tinhlv@gmail.com', N'Lê Văn Tính', N'TDMU', N'0333', 1, 0)
GO
INSERT [dbo].[Users] ([userID], [userName], [password], [email], [fullName], [address], [phone], [role], [isDeleted]) VALUES (2, N'tuyen', N'111', N'tuyen@gmail.com', N'Ngọc Tuyền', N'TDMU', N'0888', 0, 0)
GO
INSERT [dbo].[Users] ([userID], [userName], [password], [email], [fullName], [address], [phone], [role], [isDeleted]) VALUES (3, N'binh', N'111', N'binhoc@gmail.com', N'Bình Óc', N'TDMU', N'0777', 0, 0)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO



CREATE TABLE [dbo].[Categories](
	[idCategory] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[idCategory] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET IDENTITY_INSERT [dbo].[Categories] ON 

GO
INSERT [dbo].[Categories] ([idCategory], [name]) VALUES (1, N'phone')
GO
INSERT [dbo].[Categories] ([idCategory], [name]) VALUES (2, N'laptop')
GO
INSERT [dbo].[Categories] ([idCategory], [name]) VALUES (3, N'phukien')
GO
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO

CREATE TABLE [dbo].[Products](
	[idProduct] [INT] IDENTITY(1,1) NOT NULL,
	[nameProduct] [NVARCHAR](50) NOT NULL,
	[description] [TEXT] NOT NULL,
	[price] [DECIMAL](18, 0) NOT NULL,
	[discountedPrice] [DECIMAL](18, 0) NULL,
	[quantity] [INT] NOT NULL,
	[brand] [VARCHAR](20) NOT NULL,
	[category] [INT] NOT NULL,
	[image] [VARCHAR](50) NOT NULL,
	[isDiscounted] [BIT] NOT NULL,
	[isFeatured] [BIT] NOT NULL,
	[isDeleted] [BIT] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[idProduct] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_isDiscounted]  DEFAULT ((0)) FOR [isDiscounted]
GO

ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_isFeatured]  DEFAULT ((0)) FOR [isFeatured]
GO

ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_isDeleted]  DEFAULT ((0)) FOR [isDeleted]
GO

ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Categories] FOREIGN KEY([category])
REFERENCES [dbo].[Categories] ([idCategory])
GO

ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Categories]
GO

SET IDENTITY_INSERT [dbo].[Products] ON 

GO
INSERT [dbo].[Products] ([idProduct], [nameProduct], [description], [price], [discountedPrice], [quantity], [brand], [category], [image], [isDiscounted], [isFeatured], [isDeleted]) VALUES (1, N'IP 15', N'Vàng khè', CAST(30000000 AS Decimal(18, 0)), CAST(25000000 AS Decimal(18, 0)), 100, N'IP', 1, N'222222', 0, 1, 0)
GO
INSERT [dbo].[Products] ([idProduct], [nameProduct], [description], [price], [discountedPrice], [quantity], [brand], [category], [image], [isDiscounted], [isFeatured], [isDeleted]) VALUES (2, N'IP 13', N'Vô d?ch thiên h?', CAST(20000000 AS Decimal(18, 0)), CAST(15000000 AS Decimal(18, 0)), 100, N'IP', 1, N'333333', 0, 0, 0)
GO
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
