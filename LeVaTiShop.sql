use master
drop Database LeVaTiShop
GO

--Tao CSDL
create database LeVaTiShop
GO
use LeVaTiShop
GO
--Users
CREATE TABLE [dbo].[Users](
	[idUser] [int] IDENTITY(1,1) NOT NULL,
	[nameUser] [varchar](20) NOT NULL,
	[password] [varchar](20) NOT NULL,
	[email] [varchar](20) NOT NULL,
	[fullName] [nvarchar](50) NOT NULL,
	[address] [nvarchar](100) NULL,
	[phone] [varchar](10) NOT NULL,
	[avatar] VARCHAR(20) NULL,
	[role] [bit] NOT NULL,
	[isDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[idUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_isDeleted]  DEFAULT ((0)) FOR [isDeleted]
GO

SET IDENTITY_INSERT [dbo].[Users] ON 

GO
INSERT [dbo].[Users] ([idUser], [nameUser], [password], [email], [fullName], [address], [phone], [avatar], [role], [isDeleted]) VALUES (1, N'admin', N'admin', N'tinhlv@gmail.com', N'Lê Văn Tính', N'TDMU', N'0333', NULL, 1, 0)
GO
INSERT [dbo].[Users] ([idUser], [nameUser], [password], [email], [fullName], [address], [phone], [avatar], [role], [isDeleted]) VALUES (2, N'tuyen', N'111', N'tuyen@gmail.com', N'Ngọc Tuyền', N'TDMU', N'0888', NULL, 0, 0)
GO
INSERT [dbo].[Users] ([idUser], [nameUser], [password], [email], [fullName], [address], [phone], [avatar], [role], [isDeleted]) VALUES (3, N'binh', N'111', N'binhoc@gmail.com', N'Bình Óc', N'TDMU', N'0777', NULL, 0, 0)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO


--Users


--[Categories]
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
INSERT [dbo].[Categories] ([idCategory], [name]) VALUES (1, N'Điện thoại')
GO
INSERT [dbo].[Categories] ([idCategory], [name]) VALUES (2, N'Phụ kiện')
GO
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
--[Categories]


--[Message]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Message](
	[idUser] [int] NOT NULL,
	[date] [datetime] NOT NULL,
	[messageContent] [ntext] NOT NULL,
 CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED 
(
	[idUser] ASC,
	[date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_Users] FOREIGN KEY([idUser])
REFERENCES [dbo].[Users] ([idUser])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_Users]
GO

--[Message]

--[brand]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Brand](
	[idBrand] [INT] IDENTITY(1,1) NOT NULL,
	[nameBrand] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Brand] PRIMARY KEY CLUSTERED 
(
	[idBrand] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Brand] ON 

GO
INSERT dbo.Brand(idBrand, nameBrand) VALUES(1,N'iPhone')
GO
INSERT dbo.Brand(idBrand, nameBrand) VALUES(2,N'Samsung')
GO
INSERT dbo.Brand(idBrand, nameBrand) VALUES(3,N'OPPO')
GO
INSERT dbo.Brand(idBrand, nameBrand) VALUES(4,N'Realme')
GO
INSERT dbo.Brand(idBrand, nameBrand) VALUES(5,N'Xiaomi')
GO
SET IDENTITY_INSERT [dbo].[Brand] OFF
GO
--[brand]


--[Products]
CREATE TABLE [dbo].[Products](
	[idProduct] [INT] IDENTITY(1,1) NOT NULL,
	[nameProduct] [NVARCHAR](50) NOT NULL,
	[description] [NTEXT] NOT NULL,
	[price] [DECIMAL](18, 0) NOT NULL,
	[discountedPrice] [DECIMAL](18, 0) NULL,
	[inventory] [INT] NOT NULL,
	[idBrand] [INT] NOT NULL,
	[idCategory] [INT] NOT NULL,
	[image] [VARCHAR](50) NOT NULL,
	[specifications] [ntext] NOT NULL,
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

/*ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_isDiscounted]  DEFAULT ((0)) FOR [isDiscounted]
GO

ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_isFeatured]  DEFAULT ((0)) FOR [isFeatured]
GO

ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_isDeleted]  DEFAULT ((0)) FOR [isDeleted]
GO*/

ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Categories] FOREIGN KEY([idCategory])
REFERENCES [dbo].[Categories] ([idCategory])
GO

ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Categories]
GO

ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Brand] FOREIGN KEY([idBrand])
REFERENCES [dbo].[Brand] ([idBrand])
GO

ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Brand]
GO

SET IDENTITY_INSERT [dbo].[Products] ON 

GO
INSERT [dbo].[Products] ([idProduct], [nameProduct], [description], [price], [discountedPrice], [inventory], [idBrand], [idCategory], [image], [specifications], [isDiscounted], [isFeatured], [isDeleted]) VALUES (1, N'iPhone 11', N'<p>Với hiệu suất mạnh mẽ, camera tốt, và tính năng ấn tượng, <i><strong>iPhone 11</strong></i> đã trở thành một trong những mẫu điện thoại thông minh phổ biến của <strong>Apple</strong> và tiếp tục được ưa chuộng đối với người dùng trên khắp thế giới.</p>', CAST(13990000 AS Decimal(18, 0)), CAST(11490000 AS Decimal(18, 0)), 87, 1, 1, N'#1c1c1c#fdfaf5', N'@(Thẻ sim){1 Nano SIM & 1 eSIMHỗ trợ 4G}@(Ngày ra mắt){09/2019}@(Màn hình){Liquid Retina HD kích thước 6.1 inch.}@(Độ phân giải){Liquid Retina (828 x 1792 Pixels)}@(CPU){Apple A13 Bionic 6 nhân}@(RAM){4 GB}@(Bộ nhớ thẻ nhớ){64GB, 128GB, và 256GB.}@(Came sau){2 camera 12 MP}@(Came trước){12 MP}@(Jack 3.5mm.loa){3.5 mm}@(Pin){3110 mAh18 W}@(GPU){Apple GPU 4 nhân}@(Hệ điều hành){iOS 15}@(Trong lượng){Dài 150.9 mm - Ngang 75.7 mm - Dày 8.3 mm - Nặng 194 g}@(Chuẩn bộ nhớ){MicroSD, hỗ trợ tối đa 1 TB}', 1, 1, 0)
GO
INSERT [dbo].[Products] ([idProduct], [nameProduct], [description], [price], [discountedPrice], [inventory], [idBrand], [idCategory], [image], [specifications], [isDiscounted], [isFeatured], [isDeleted]) VALUES (2, N'iPhone 12', N'<p>Phone 12 là một trong những mẫu iPhone đầu tiên hỗ trợ kết nối 5G, giúp tốc độ internet nhanh hơn và ổn định hơn. Với thiết kế mới, hiệu suất mạnh mẽ, camera nâng cấp, và hỗ trợ 5G, iPhone 12 đã trở thành một trong những mẫu điện thoại thông minh được mong đợi nhất và tiếp tục được ưa chuộng rộng rãi.</p>', CAST(18990000 AS Decimal(18, 0)), CAST(15390000 AS Decimal(18, 0)), 100, 1, 1, N'#b5ace3#d1e4d0', N'@(Thẻ sim){1 Nano SIM & 1 eSIMHỗ trợ 5G}@(Ngày ra mắt){10/2020}@(Màn hình){Super Retina XDR kích thước 6.1 inch}@(Độ phân giải){Super Retina XDR (1170 x 2532 Pixels)}@(CPU){2 nhân 3.1 GHz & 4 nhân 1.8 GHz}@(RAM){4 GB}@(Bộ nhớ thẻ nhớ){64GB, 128GB, và 256GB.}@(Came sau){2 camera 12 MP}@(Came trước){12 MP}@(Jack 3.5mm.loa){3.5 mm}@(Pin){2815 mAh20 W}@(GPU){Apple GPU 4 nhân}@(Hệ điều hành){iOS 15}@(Trong lượng){Dài 146.7 mm - Ngang 71.5 mm - Dày 7.4 mm - Nặng 164 g}@(Chuẩn bộ nhớ){MicroSD, hỗ trợ tối đa 1 TB}', 1, 1, 0)
GO
INSERT [dbo].[Products] ([idProduct], [nameProduct], [description], [price], [discountedPrice], [inventory], [idBrand], [idCategory], [image], [specifications], [isDiscounted], [isFeatured], [isDeleted]) VALUES (3, N'iPhone 13', N'<p><strong>iPhone 13</strong> được thiết kế với màn hình<i><strong> Super Retina XDR</strong></i> tiếp tục cải thiện về độ sáng và chất lượng hình ảnh. Màn hình có kích thước khác nhau tùy theo phiên bản, ví dụ: <strong>iPhone 13 Mini</strong> có màn hình 5.4 inch, còn <strong>iPhone 13 Pro Max</strong> có màn hình lớn 6.7 inch. Thiết kế của <strong>iPhone 13</strong> tiếp tục sử dụng khung viền bằng thép không gỉ và mặt trước bằng kính, tạo nên sự sang trọng và bền bỉ.<br>&nbsp;</p>', CAST(21990000 AS Decimal(18, 0)), CAST(18899000 AS Decimal(18, 0)), 20, 1, 1, N'#215e7b#161d23#f9dcd6#fbfaf7', N'@(Thẻ sim){1 Nano SIM & 1 eSIMHỗ trợ 5G}@(Ngày ra mắt){09/2021}@(Màn hình){OLED6.1"Super Retina XDR}@(Độ phân giải){Super Retina XDR (1170 x 2532 Pixels)}@(CPU){3.22 GHz}@(RAM){4 GB}@(Bộ nhớ thẻ nhớ){64GB, 128GB, và 256GB.}@(Came sau){2 camera 12 MP}@(Came trước){12 MP}@(Jack 3.5mm.loa){3.5 mm}@(Pin){3240 mAh20 W}@(GPU){Apple GPU 4 nhân}@(Hệ điều hành){iOS 15}@(Trong lượng){Dài 146.7 mm - Ngang 71.5 mm - Dày 7.65 mm - Nặng 173 g}@(Chuẩn bộ nhớ){MicroSD, hỗ trợ tối đa 1 TB}', 1, 1, 0)
GO
INSERT [dbo].[Products] ([idProduct], [nameProduct], [description], [price], [discountedPrice], [inventory], [idBrand], [idCategory], [image], [specifications], [isDiscounted], [isFeatured], [isDeleted]) VALUES (4, N'iPhone 14', N'<p><i><strong>iPhone 14</strong></i> chạy trên hệ điều hành iOS tiên tiến nhất, cung cấp trải nghiệm người dùng mượt mà và hỗ trợ hàng nghìn ứng dụng đa dạng từ App Store. Máy có pin dung lượng lớn hơn để đảm bảo sử dụng cả ngày mà không cần sạc lại. Hỗ trợ cả sạc không dây và sạc nhanh.<br>&nbsp;</p>', CAST(30990000 AS Decimal(18, 0)), CAST(27990000 AS Decimal(18, 0)), 33, 1, 1, N'#1a2129#9cb1c4#e00523#ebe3f0#f9f6f1', N'@(Thẻ sim){1 Nano SIM & 1 eSIMHỗ trợ 5G}@(Ngày ra mắt){09/2022}@(Màn hình){OLED6.1"Super Retina XDR}@(Độ phân giải){Super Retina XDR (1170 x 2532 Pixels)}@(CPU){Apple A15 Bionic 6 nhân}@(RAM){6 GB}@(Bộ nhớ thẻ nhớ){64GB, 128GB, và 256GB.}@(Came sau){2 camera 12 MP}@(Came trước){12 MP}@(Jack 3.5mm.loa){3.5 mm}@(Pin){3279 mAh20 W}@(GPU){Apple GPU 5 nhân}@(Hệ điều hành){iOS 16}@(Trong lượng){Dài 146.7 mm - Ngang 71.5 mm - Dày 7.8 mm - Nặng 172 g}@(Chuẩn bộ nhớ){MicroSD, hỗ trợ tối đa 1 TB}', 1, 1, 0)
GO
INSERT [dbo].[Products] ([idProduct], [nameProduct], [description], [price], [discountedPrice], [inventory], [idBrand], [idCategory], [image], [specifications], [isDiscounted], [isFeatured], [isDeleted]) VALUES (5, N'Samsung Galaxy M34 5g', N'<p><i><strong>Galaxy M34 5G</strong></i> có một thiết kế khá đặc trưng giống với phần lớn những mẫu điện thoại được <strong>Samsung</strong> ra mắt trong năm <strong>2023</strong>, vẫn là mặt lưng và màn hình phẳng sang trọng đi cùng với kiểu bố trí cụm camera xếp dọc quen thuộc.</p>', CAST(7990000 AS Decimal(18, 0)), CAST(7499000 AS Decimal(18, 0)), 45, 2, 1, N'#94b4bf#1d2031', N'@(Thẻ sim){2 Nano SIM (SIM 2 chung khe thẻ nhớ)Hỗ trợ 5G}@(Ngày ra mắt){10/2023}@(Màn hình){Super AMOLED6.5"Full HD+}@(Độ phân giải){Full HD+ (1080 x 2340 Pixels)}@(CPU){2 nhân 2.4 GHz & 6 nhân 2 GHz}@(RAM){8 GB}@(Bộ nhớ thẻ nhớ){256 GB,512GB}@(Came sau){Chính 50 MP & Phụ 8 MP, 2 MP}@(Came trước){13 MP}@(Jack 3.5mm.loa){3.5 mm}@(Pin){6000 mAh25 W}@(GPU){Mali-G68}@(Hệ điều hành){Android 13}@(Trong lượng){Dài 161.7 mm - Ngang 77.2 mm - Dày 8.8 mm - Nặng 208 g}@(Chuẩn bộ nhớ){MicroSD, hỗ trợ tối đa 1 TB}', 1, 1, 0)
GO
INSERT [dbo].[Products] ([idProduct], [nameProduct], [description], [price], [discountedPrice], [inventory], [idBrand], [idCategory], [image], [specifications], [isDiscounted], [isFeatured], [isDeleted]) VALUES (6, N'Samsung Galaxy A04s', N'<p><i><strong>Galaxy A04s</strong></i> được lấy cảm hứng thiết kế đến từ những chiếc&nbsp;điện thoại <strong>Galaxy dòng A</strong>&nbsp;trong năm <strong>2023</strong>, với mặt lưng đơn sắc cùng kiểu bố trí cụm camera xếp dọc riêng lẻ làm điểm nhấn. Những điều này tạo nên một sự tối giản trong thiết kế, cùng độ nhận diện cao cho chiếc smartphone giá rẻ nhà <strong>Samsung</strong>. Đối với chiếc máy này thì hãng cho ra mắt 3 phiên bản màu đó là đen, nâu và xanh lá đậm, phù hợp với nhiều đối tượng người dùng khác nhau ở mọi độ tuổi.</p>', CAST(33990000 AS Decimal(18, 0)), CAST(24990000 AS Decimal(18, 0)), 20, 2, 1, N'#26272c#9a5a5a#294946', N'@(Thẻ sim){2 Nano SIMHỗ trợ 4G}@(Ngày ra mắt){09/2022}@(Màn hình){IPS LCD6.5"HD+}@(Độ phân giải){HD+ (720 x 1600 Pixels)}@(CPU){Exynos 850 8 nhân}@(RAM){4 GB}@(Bộ nhớ thẻ nhớ){128GB, 256GB}@(Came sau){Chính 50 MP & Phụ 2 MP, 2 MP}@(Came trước){5 MP}@(Jack 3.5mm.loa){3.5 mm}@(Pin){5000 mAh15 W}@(GPU){Mali-G52}@(Hệ điều hành){Android 12}@(Trong lượng){Dài 164.7 mm - Ngang 76.7 mm - Dày 9.1 mm - Nặng 195 g}@(Chuẩn bộ nhớ){MicroSD, hỗ trợ tối đa 1 TB}', 1, 1, 0)
GO
INSERT [dbo].[Products] ([idProduct], [nameProduct], [description], [price], [discountedPrice], [inventory], [idBrand], [idCategory], [image], [specifications], [isDiscounted], [isFeatured], [isDeleted]) VALUES (7, N'Samsung Galaxy A14 4GB', N'<p><i><strong>Samsung Galaxy A14 4G</strong></i> được thiết kế nguồn cảm hứng đến từ dòng sản phẩm cao cấp <strong>Galaxy S23 series</strong>. Với thiết kế hiện đại, màu sắc thanh lịch và góc cạnh bo tròn tinh tế, máy mang đến cho người dùng một cái nhìn cuốn hút đầy sang trọng. Bất kỳ ai sử dụng chiếc điện thoại <strong>Samsung</strong> này đều có thể thưởng thức sự tinh tế của thiết kế, nâng cao phong cách và cá tính của mình.</p>', CAST(6290000 AS Decimal(18, 0)), CAST(4990000 AS Decimal(18, 0)), 16, 2, 1, N'#afa8b1#1d1e23#653f4c', N'@(Thẻ sim){2 Nano SIMHỗ trợ 4G}@(Ngày ra mắt){03/2023}@(Màn hình){PLS LCD6.6"Full HD+}@(Độ phân giải){Full HD+ (1080 x 2408 Pixels)}@(CPU){Exynos 850 8 nhân}@(RAM){4 GB}@(Bộ nhớ thẻ nhớ){8GB, 6GB}@(Came sau){Chính 50 MP & Phụ 5 MP, 2 MP}@(Came trước){13 MP}@(Jack 3.5mm.loa){3.5 mm}@(Pin){5000 mAh15 W}@(GPU){Mali-G52}@(Hệ điều hành){Android 13}@(Trong lượng){Dài 167.7 mm - Ngang 78 mm - Dày 9.1 mm - Nặng 201 g}@(Chuẩn bộ nhớ){MicroSD, hỗ trợ tối đa 1 TB}', 1, 0, 0)
GO
INSERT [dbo].[Products] ([idProduct], [nameProduct], [description], [price], [discountedPrice], [inventory], [idBrand], [idCategory], [image], [specifications], [isDiscounted], [isFeatured], [isDeleted]) VALUES (8, N'Samsung Galaxy Z Fold5 5G 256GB', N'<p><i><strong>Samsung Galaxy Z Fold5</strong></i> vẫn tiếp tục giữ nguyên thiết kế gập độc đáo dạng ngang của dòng <i><strong>Galaxy Z Fold</strong></i> trước đó. Với kích thước nhỏ gọn khi gập lại, điện thoại này trở nên dễ dàng mang theo và cất giữ trong túi áo hay túi xách của bạn. Khi mở ra, <i><strong>Galaxy Z Fold5</strong></i> trở thành một chiếc điện thoại thông thường với màn hình lớn hơn, mang đến trải nghiệm sử dụng rộng lớn và ấn tượng.</p>', CAST(41990000 AS Decimal(18, 0)), CAST(36990000 AS Decimal(18, 0)), 10, 2, 1, N'#151412#ece8df#a1a8ba', N'@(Thẻ sim){2 Nano SIM hoặc 1 Nano SIM + 1 eSIMHỗ trợ 5G}@(Ngày ra mắt){07/2023}@(Màn hình){Dynamic AMOLED 2XChính 7.6" & Phụ 6.2"Quad HD+ (2K+)}@(Độ phân giải){Chính: QXGA+ (2176 x 1812 Pixels) & Phụ: HD+ (2316 x 904 Pixels)}@(CPU){Snapdragon 8 Gen 2 for Galaxy}@(RAM){12 GB}@(Bộ nhớ thẻ nhớ){6GB-128}@(Came sau){Chính 50 MP & Phụ 12 MP, 10 MP}@(Came trước){10 MP & 4 MP}@(Jack 3.5mm.loa){3.5 mm}@(Pin){4400 mAh25 W}@(GPU){Adreno 740}@(Hệ điều hành){Android 13}@(Trong lượng){Dài 154.9 mm - Ngang 129.9 mm - Dày 6.1 mm - Nặng 253 g}@(Chuẩn bộ nhớ){MicroSD, hỗ trợ tối đa 1 TB}', 1, 1, 0)
GO
INSERT [dbo].[Products] ([idProduct], [nameProduct], [description], [price], [discountedPrice], [inventory], [idBrand], [idCategory], [image], [specifications], [isDiscounted], [isFeatured], [isDeleted]) VALUES (9, N'Samsung Galaxy A54 5G 128GB', N'<p><i><strong>Samsung Galaxy A54 5G 128GB</strong></i> là mẫu điện thoại tầm trung hiện đang làm mưa làm gió tại thị trường Việt Nam khi sở hữu những thông số ấn tượng với chip hiệu năng cao, màn hình chất lượng đi cùng hệ thống camera xịn sò trong phân khúc.</p>', CAST(10490000 AS Decimal(18, 0)), CAST(8990000 AS Decimal(18, 0)), 44, 2, 1, N'#2f3032#9392d8#e4f5c1', N'@(Thẻ sim){2 Nano SIMHỗ trợ 5G}@(Ngày ra mắt){03/2023}@(Màn hình){Super AMOLED6.4"Full HD+}@(Độ phân giải){Full HD+ (1080 x 2340 Pixels)}@(CPU){Exynos 1380 8 nhân}@(RAM){8 GB}@(Bộ nhớ thẻ nhớ){8GB-256GB}@(Came sau){Chính 50 MP & Phụ 12 MP, 5 MP}@(Came trước){32 MP}@(Jack 3.5mm.loa){3.5 mm}@(Pin){5000 mAh33 W}@(GPU){Mali-G68}@(Hệ điều hành){Android 13}@(Trong lượng){Dài 158.2 mm - Ngang 76.7 mm - Dày 8.2 mm - Nặng 202 g}@(Chuẩn bộ nhớ){MicroSD, hỗ trợ tối đa 1 TB}', 1, 0, 0)
GO
INSERT [dbo].[Products] ([idProduct], [nameProduct], [description], [price], [discountedPrice], [inventory], [idBrand], [idCategory], [image], [specifications], [isDiscounted], [isFeatured], [isDeleted]) VALUES (10, N'OPPO A77s', N'<p><i><strong>OPPO A77s</strong></i> có một vẻ ngoài khá đẹp mắt khi được tạo hình theo kiểu vuông vức và cách phối màu ở mặt lưng độc đáo. Khung và mặt lưng phẳng giúp tôn lên vẻ thời thượng cho điện thoại, bởi đây được xem là kiểu dáng xu hướng và được người dùng khá ưa chuộng trong vài năm gần đây.</p>', CAST(6299000 AS Decimal(18, 0)), CAST(5490000 AS Decimal(18, 0)), 33, 3, 1, N'#15181f#b6cfdf', N'@(Thẻ sim){2 Nano SIMHỗ trợ 4G}@(Ngày ra mắt){10/2022}@(Màn hình){IPS LCD6.56"HD+}@(Độ phân giải){HD+ (720 x 1612 Pixels)}@(CPU){2.4 GHz}@(RAM){8 GB}@(Bộ nhớ thẻ nhớ){}@(Came sau){Chính 50 MP & Phụ 2 MP}@(Came trước){8 MP}@(Jack 3.5mm.loa){3.5 mm}@(Pin){5000 mAh10 W}@(GPU){Adreno 610}@(Hệ điều hành){Android 12}@(Trong lượng){Dài 163.74 mm - Ngang 75.03 mm - Dày 7.99 mm - Nặng 187 g}@(Chuẩn bộ nhớ){MicroSD, hỗ trợ tối đa 2 TB}', 0, 0, 0)
GO
INSERT [dbo].[Products] ([idProduct], [nameProduct], [description], [price], [discountedPrice], [inventory], [idBrand], [idCategory], [image], [specifications], [isDiscounted], [isFeatured], [isDeleted]) VALUES (11, N'OPPO A17', N'<p><i><strong>OPPO A17</strong></i> là mẫu điện thoại thuộc phân khúc giá rẻ, máy thu hút nhiều sự chú ý khi sở hữu cấu hình tốt, thiết kế đẹp cùng mức giá bán phải chăng, hứa hẹn mang lại nhiều trải nghiệm tốt “<i>đáng tiền hơn giá tiền</i>”.<br>&nbsp;</p>', CAST(8990000 AS Decimal(18, 0)), CAST(8490000 AS Decimal(18, 0)), 65, 3, 1, N'#374051#c8b897', N'@(Thẻ sim){2 Nano SIMHỗ trợ 4G}@(Ngày ra mắt){10/2022}@(Màn hình){IPS LCD6.56"HD+}@(Độ phân giải){HD+ (720 x 1612 Pixels)}@(CPU){MediaTek Helio G35 8 nhân}@(RAM){4 GB}@(Bộ nhớ thẻ nhớ){}@(Came sau){Chính 50 MP & Phụ 0.3 MP}@(Came trước){5 MP}@(Jack 3.5mm.loa){3.5 mm}@(Pin){5000 mAh33 W}@(GPU){IMG PowerVR GE8320}@(Hệ điều hành){Android 12}@(Trong lượng){Dài 164.2 mm - Ngang 75.6 mm - Dày 8.3 mm - Nặng 189 g}@(Chuẩn bộ nhớ){MicroSD, hỗ trợ tối đa 1 TB}', 1, 1, 0)
GO
INSERT [dbo].[Products] ([idProduct], [nameProduct], [description], [price], [discountedPrice], [inventory], [idBrand], [idCategory], [image], [specifications], [isDiscounted], [isFeatured], [isDeleted]) VALUES (12, N'OPPO A38', N'<p><i><strong>OPPO A38</strong></i> mẫu điện thoại tầm trung mới nhất được <strong>OPPO</strong> mang đến cho người tiêu dùng vào nửa cuối năm <strong>2023</strong>. Máy sở hữu lối thiết kế quen thuộc của dòng điện thoại <strong>OPPO A</strong>, đồng thời có một hiệu năng ổn định cùng màn hình hiển thị sắc nét chắc chắn sẽ không làm bạn thất vọng.<br>&nbsp;</p>', CAST(6990000 AS Decimal(18, 0)), CAST(6290000 AS Decimal(18, 0)), 34, 3, 1, N'#212226#fdcca3', N'@(Thẻ sim){2 Nano SIMHỗ trợ 4G}@(Ngày ra mắt){09/2023}@(Màn hình){IPS LCD6.56"HD+}@(Độ phân giải){HD+ (720 x 1612 Pixels)}@(CPU){MediaTek Helio G85 8 nhân}@(RAM){4 GB}@(Bộ nhớ thẻ nhớ){}@(Came sau){Chính 50 MP & Phụ 2 MP}@(Came trước){5 MP}@(Jack 3.5mm.loa){3.5 mm}@(Pin){5000 mAh33 W}@(GPU){Mali-G52 MP2}@(Hệ điều hành){Android 13}@(Trong lượng){Dài 163.74 mm - Ngang 75.03 mm - Dày 8.16 mm - Nặng 190 g}@(Chuẩn bộ nhớ){MicroSD, hỗ trợ tối đa 2 TB}', 1, 0, 0)
GO
INSERT [dbo].[Products] ([idProduct], [nameProduct], [description], [price], [discountedPrice], [inventory], [idBrand], [idCategory], [image], [specifications], [isDiscounted], [isFeatured], [isDeleted]) VALUES (13, N'OPPO A58 8GB', N'<p>Thị trường điện thoại di động ngày nay, <i><strong>OPPO A58 8GB</strong></i> là một trong những sản phẩm nổi bật với thiết kế vuông vức và hiện đại. Được thiết kế với mục tiêu tối ưu hóa trải nghiệm người dùng, chiếc điện thoại này mang đến một loạt tính năng ấn tượng trong một thiết kế thon gọn và nhẹ nhàng.</p>', CAST(13990000 AS Decimal(18, 0)), CAST(11990000 AS Decimal(18, 0)), 56, 3, 1, N'#1d2226#b7dad6', N'@(Thẻ sim){2 Nano SIMHỗ trợ 4G}@(Ngày ra mắt){08/2023}@(Màn hình){LTPS LCD6.72"Full HD+}@(Độ phân giải){Full HD+ (1080 x 2412 Pixels)}@(CPU){MediaTek Helio G85 8 nhân}@(RAM){8 GB}@(Bộ nhớ thẻ nhớ){}@(Came sau){Chính 50 MP & Phụ 2 MP}@(Came trước){8 MP}@(Jack 3.5mm.loa){3.5 mm}@(Pin){5000 mAh67 W}@(GPU){Mali-G52 MP2}@(Hệ điều hành){Android 13}@(Trong lượng){Dài 165.65 mm - Ngang 75.98 mm - Dày 7.99 mm - Nặng 192 g}@(Chuẩn bộ nhớ){MicroSD, hỗ trợ tối đa 1 TB}', 0, 1, 0)
GO
INSERT [dbo].[Products] ([idProduct], [nameProduct], [description], [price], [discountedPrice], [inventory], [idBrand], [idCategory], [image], [specifications], [isDiscounted], [isFeatured], [isDeleted]) VALUES (14, N'OPPO A98 5G', N'<p>Những mẫu điện thoại <strong>OPPO</strong> cho ra mắt thời gian gần đây (<strong>2023</strong>) có vẻ ngoài đẹp mắt phù hợp với thị hiếu người tiêu dùng hiện nay. Trong đó <i><strong>OPPO A98 5G</strong></i> mẫu điện thoại mới của điện thoại <strong>OPPO A</strong>, với lối thiết kế hiện đại, màn hình hiển thị chi tiết thông tin cũng như một hiệu năng ổn định.</p>', CAST(8990000 AS Decimal(18, 0)), CAST(8690000 AS Decimal(18, 0)), 88, 3, 1, N'#242d36#a3dee4', N'@(Thẻ sim){2 Nano SIM (SIM 2 chung khe thẻ nhớ)Hỗ trợ 5G}@(Ngày ra mắt){06/2023}@(Màn hình){LTPS LCD6.72"Full HD+}@(Độ phân giải){Full HD+ (1080 x 2400 Pixels)}@(CPU){Snapdragon 695 5G 8 nhân}@(RAM){8 GB}@(Bộ nhớ thẻ nhớ){}@(Came sau){Chính 64 MP & Phụ 2 MP, 2 MP}@(Came trước){32 MP}@(Jack 3.5mm.loa){3.5 mm}@(Pin){5000 mAh67 W}@(GPU){Adreno 619}@(Hệ điều hành){Android 13}@(Trong lượng){Dài 165.6 mm - Ngang 76.1 mm - Dày 8.2 mm - Nặng 192 g}@(Chuẩn bộ nhớ){MicroSD, hỗ trợ tối đa 1 TB}', 1, 0, 0)
GO
INSERT [dbo].[Products] ([idProduct], [nameProduct], [description], [price], [discountedPrice], [inventory], [idBrand], [idCategory], [image], [specifications], [isDiscounted], [isFeatured], [isDeleted]) VALUES (15, N'OPPO A78', N'<p><i><strong>OPPO A78</strong></i> một sản phẩm được nhà <strong>OPPO</strong> cho ra mắt với thiết kế trẻ trung, thiết bị này được đánh giá có hiệu năng ổn định, màn hình sắc nét và viên pin có dung lượng lớn, phù hợp cho người dùng sử dụng lâu dài.</p>', CAST(4690000 AS Decimal(18, 0)), CAST(3990000 AS Decimal(18, 0)), 77, 3, 1, N'#313131#7ed1bf', N'@(Thẻ sim){2 Nano SIMHỗ trợ 4G}@(Ngày ra mắt){07/2023}@(Màn hình){AMOLED6.43"Full HD+}@(Độ phân giải){Full HD+ (1080 x 2400 Pixels)}@(CPU){4 nhân 2.4 GHz & 4 nhân 1.9 GHz}@(RAM){8 GB}@(Bộ nhớ thẻ nhớ){}@(Came sau){Chính 50 MP & Phụ 2 MP}@(Came trước){8 MP}@(Jack 3.5mm.loa){3.5 mm}@(Pin){4600 mAh80 W}@(GPU){Adreno 610}@(Hệ điều hành){Android 13}@(Trong lượng){Dài 160 mm - Ngang 73.23 mm - Dày 7.93 mm - Nặng 180 g}@(Chuẩn bộ nhớ){MicroSD, hỗ trợ tối đa 1 TB}', 1, 0, 0)
GO
INSERT [dbo].[Products] ([idProduct], [nameProduct], [description], [price], [discountedPrice], [inventory], [idBrand], [idCategory], [image], [specifications], [isDiscounted], [isFeatured], [isDeleted]) VALUES (16, N'OPPO Reno10 Pro 5G', N'<p>Thiết kế của <i><strong>OPPO Reno10 Pro 5G</strong></i> trông rất ấn tượng với kiểu thiết kế bo cong ở mặt lưng và màn hình, điều này mang lại trải nghiệm cầm nắm thoải mái khi sử dụng để không cảm thấy bị cấn, phần viền màn hình vì thế cũng trông mỏng hơn giúp tạo nên một tổng thể sang trọng và cực kỳ bắt mắt.</p>', CAST(13990000 AS Decimal(18, 0)), CAST(12490000 AS Decimal(18, 0)), 33, 3, 1, N'#3b3b39#9d88b1', N'@(Thẻ sim){2 Nano SIMHỗ trợ 5G}@(Ngày ra mắt){08/2023}@(Màn hình){AMOLED6.7"Full HD+}@(Độ phân giải){Full HD+ (1080 x 2412 Pixels)}@(CPU){Snapdragon 778G 5G 8 nhân}@(RAM){12 GB}@(Bộ nhớ thẻ nhớ){}@(Came sau){Chính 50 MP & Phụ 32 MP, 8 MP}@(Came trước){32 MP}@(Jack 3.5mm.loa){3.5 mm}@(Pin){5000 mAh33 W}@(GPU){Adreno 642L}@(Hệ điều hành){Android 13}@(Trong lượng){Dài 162.3 mm - Ngang 74.2 mm - Dày 7.89 mm - Nặng 185 g}@(Chuẩn bộ nhớ){MicroSD, hỗ trợ tối đa 1 TB}', 1, 0, 0)
GO
INSERT [dbo].[Products] ([idProduct], [nameProduct], [description], [price], [discountedPrice], [inventory], [idBrand], [idCategory], [image], [specifications], [isDiscounted], [isFeatured], [isDeleted]) VALUES (17, N'realme C55 6GB', N'<p><i><strong>Realme C55</strong></i> là mẫu máy giá rẻ được mở bán chính thức tại thị trường Việt Nam vào tháng <strong>03/2023</strong>. Điện thoại dành được khá nhiều sự quan tâm của đông đảo người dùng khi mở bán với mức giá hấp dẫn, trang bị cấu hình tốt, <strong>camera AI 64 MP</strong>, màn hình lớn cùng khả năng sạc pin siêu nhanh.</p>', CAST(4790000 AS Decimal(18, 0)), CAST(3790000 AS Decimal(18, 0)), 45, 4, 1, N'#293241#fdedd4', N'@(Thẻ sim){2 Nano SIMHỗ trợ 4G}@(Ngày ra mắt){03/2023}@(Màn hình){IPS LCD6.72"Full HD+}@(Độ phân giải){Full HD+ (1080 x 2400 Pixels)}@(CPU){MediaTek Helio G88 8 nhân}@(RAM){6 GB}@(Bộ nhớ thẻ nhớ){}@(Came sau){Chính 64 MP & Phụ 2 MP}@(Came trước){8 MP}@(Jack 3.5mm.loa){}@(Pin){5000 mAh33 W}@(GPU){Mali-G52}@(Hệ điều hành){Android 13}@(Trong lượng){Dài 165.65 mm - Ngang 75.98 mm - Dày 7.89 mm - Nặng 189.5 g}@(Chuẩn bộ nhớ){}', 1, 0, 0)
GO
INSERT [dbo].[Products] ([idProduct], [nameProduct], [description], [price], [discountedPrice], [inventory], [idBrand], [idCategory], [image], [specifications], [isDiscounted], [isFeatured], [isDeleted]) VALUES (18, N'realme C53', N'<p><i><strong>realme C53</strong></i> được thiết kế với sự vuông vắn và làm chủ yếu từ nhựa, mang lại cảm giác chắc chắn khi cầm nắm. Với màn hình giọt nước nhỏ gọn, chiếc điện thoại <strong>realme </strong>này có thể cung cấp cho người dùng trải nghiệm màn hình rộng hơn mà vẫn giữ được kích thước nhỏ gọn và thời trang.</p>', CAST(11990000 AS Decimal(18, 0)), CAST(10990000 AS Decimal(18, 0)), 21, 4, 1, N'#1b1e23#fbe9a8', N'@(Thẻ sim){2 Nano SIMHỗ trợ 4G}@(Ngày ra mắt){05/2023}@(Màn hình){IPS LCD6.74"HD+}@(Độ phân giải){HD+ (720 x 1600 Pixels)}@(CPU){Unisoc Tiger T612}@(RAM){6 GB}@(Bộ nhớ thẻ nhớ){}@(Came sau){Chính 50 MP & Phụ 0.08 MP}@(Came trước){8 MP}@(Jack 3.5mm.loa){}@(Pin){5000 mAh33 W}@(GPU){Mali-G57}@(Hệ điều hành){Android 13}@(Trong lượng){Dài 167.3 mm - Ngang 76.7 mm - Dày 7.49 mm - Nặng 182 g}@(Chuẩn bộ nhớ){}', 1, 0, 0)
GO
INSERT [dbo].[Products] ([idProduct], [nameProduct], [description], [price], [discountedPrice], [inventory], [idBrand], [idCategory], [image], [specifications], [isDiscounted], [isFeatured], [isDeleted]) VALUES (19, N'Xiaomi Redmi Note 12S', N'<p><i><strong>Xiaomi Redmi Note 12S</strong></i> sẽ là chiếc điện thoại tiếp theo được nhà <strong>Xiaomi </strong>tung ra thị trường Việt Nam trong thời gian tới (<strong>05/2023</strong>). Điện thoại sở hữu một lối thiết kế hiện đại, màn hình hiển thị chi tiết đi cùng với đó là một hiệu năng mượt mà xử lý tốt các tác vụ.</p>', CAST(6690000 AS Decimal(18, 0)), CAST(5890000 AS Decimal(18, 0)), 12, 5, 1, N'#88b0d4#1e1e1e#c0f3d2', N'@(Thẻ sim){2 Nano SIMHỗ trợ 4G}@(Ngày ra mắt){05/2023}@(Màn hình){AMOLED6.43"Full HD+}@(Độ phân giải){Full HD+ (1080 x 2400 Pixels)}@(CPU){MediaTek Helio G96 8 nhân}@(RAM){8 GB}@(Bộ nhớ thẻ nhớ){}@(Came sau){Chính 108 MP & Phụ 8 MP, 2 MP}@(Came trước){16 MP}@(Jack 3.5mm.loa){}@(Pin){5000 mAh33 W}@(GPU){Adreno 610}@(Hệ điều hành){Android 13}@(Trong lượng){Dài 159.87 mm - Ngang 73.87 mm - Dày 8.09 mm - Nặng 176 g}@(Chuẩn bộ nhớ){}', 0, 1, 0)
GO
INSERT [dbo].[Products] ([idProduct], [nameProduct], [description], [price], [discountedPrice], [inventory], [idBrand], [idCategory], [image], [specifications], [isDiscounted], [isFeatured], [isDeleted]) VALUES (20, N'Xiaomi Redmi Note 12 Pro', N'<p>Mới đây nhà <strong>Xiaomi </strong>cũng đã tiếp tục cho ra mắt mẫu điện thoại mới của mình trong năm <strong>2023 </strong>với tên gọi <i><strong>Xiaomi Redmi Note 12 Pro 4G 256GB</strong></i>. Được định hình là dòng sản phẩm thuộc phân khúc tầm trung, sở hữu nhiều ưu điểm về thiết kế, hiệu năng <strong>Snapdragon 732G</strong> cùng hệ thống 4 ống kính đa dụng.</p>', CAST(8790000 AS Decimal(18, 0)), CAST(7990000 AS Decimal(18, 0)), 24, 5, 1, N'#34373e#7ca09c#fedba1#c0e7fd', N'@(Thẻ sim){2 Nano SIM (SIM 2 chung khe thẻ nhớ)Hỗ trợ 4G}@(Ngày ra mắt){05/2023}@(Màn hình){AMOLED6.67"Full HD+}@(Độ phân giải){Full HD+ (1080 x 2400 Pixels)}@(CPU){Snapdragon 732G 8 nhân}@(RAM){8 GB}@(Bộ nhớ thẻ nhớ){}@(Came sau){Chính 108 MP & Phụ 8 MP, 2 MP, 2 MP}@(Came trước){16 MP}@(Jack 3.5mm.loa){}@(Pin){}@(GPU){}@(Hệ điều hành){Android 11}@(Trong lượng){Dài 164.2 mm - Ngang 76.1 mm - Dày 8.12 mm - Nặng 201.8 g}@(Chuẩn bộ nhớ){}', 1, 1, 0)
GO
INSERT [dbo].[Products] ([idProduct], [nameProduct], [description], [price], [discountedPrice], [inventory], [idBrand], [idCategory], [image], [specifications], [isDiscounted], [isFeatured], [isDeleted]) VALUES (21, N'Realme GT Neo2', N'<p><i><strong>Realme GT Neo2</strong></i>&nbsp;chính thức được trình làng, tiếp tục nhắm tới thị trường&nbsp;<strong>smartphone </strong>chơi game&nbsp;tầm trung với phong cách thiết kế hoàn toàn mới lạ, <strong>3 camera 64 MP AI</strong> chuyên nghiệp cùng sức mạnh hiệu năng vượt trội từ <strong>Snapdragon</strong> và pin "trâu" để bạn thỏa sức giải trí.</p>', CAST(9990000 AS Decimal(18, 0)), CAST(8290000 AS Decimal(18, 0)), 57, 4, 1, N'#bee36b', N'@(Thẻ sim){2 Nano SIM}@(Ngày ra mắt){10/2021}@(Màn hình){AMOLED6.62" - Tần số quét 120 Hz}@(Độ phân giải){Full HD+ (1080 x 2400 Pixels)}@(CPU){Snapdragon 870 5G 8 nhân}@(RAM){8 GB}@(Bộ nhớ thẻ nhớ){128 GB}@(Came sau){Chính 64 MP & Phụ 8 MP, 2 MP}@(Came trước){16 MP}@(Jack 3.5mm.loa){Type-C}@(Pin){5000 mAh65 W}@(GPU){Adreno 650}@(Hệ điều hành){Android 11}@(Trong lượng){Dài 162.9 mm - Ngang 75.8 mm - Dày 9 mm - Nặng 199.8 g}@(Chuẩn bộ nhớ){MicroSD, hỗ trợ tối đa 1 TB}', 1, 1, 0)
GO
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
--[Products]
--Cart
/*SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[idCart] [int] IDENTITY(1,1) NOT NULL,
	[idUser] [int] NOT NULL,
	[idProduct] [int] NOT NULL,
	[amount] [int] NOT NULL,
 CONSTRAINT [PK_Cart] PRIMARY KEY CLUSTERED 
(
	[idCart] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD  CONSTRAINT [FK_Cart_Products] FOREIGN KEY([idProduct])
REFERENCES [dbo].[Products] ([idProduct])
GO
ALTER TABLE [dbo].[Cart] CHECK CONSTRAINT [FK_Cart_Products]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD  CONSTRAINT [FK_Cart_Users] FOREIGN KEY([idUser])
REFERENCES [dbo].[Users] ([idUser])
GO
ALTER TABLE [dbo].[Cart] CHECK CONSTRAINT [FK_Cart_Users]
GO*/
--Cart
--order
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[idOrder] [int] IDENTITY(1,1) NOT NULL,
	[idUser] [int] NOT NULL,
	[state] [int] NOT NULL,
	[isPay] [bit] NOT NULL,
	[dateOrder] [datetime] NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[idOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Users] FOREIGN KEY([idUser])
REFERENCES [dbo].[Users] ([idUser])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Users]
GO

SET IDENTITY_INSERT [dbo].[Orders] ON 

GO
INSERT [dbo].[Orders] ([idOrder], [idUser], [state], [isPay], [dateOrder]) VALUES (1, 3, 2, 1, CAST(N'2023-10-10 00:00:00.000' AS DateTime))
GO
INSERT [dbo].[Orders] ([idOrder], [idUser], [state], [isPay], [dateOrder]) VALUES (2, 3, 0, 0, CAST(N'2023-10-15 00:00:00.000' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO

--order

--detailorder
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetailOrder](
	[idOrder] [int] NOT NULL,
	[idProduct] [int] NOT NULL,
	[quantity] [int] NOT NULL,
 CONSTRAINT [PK_DetailOrder] PRIMARY KEY CLUSTERED 
(
	[idOrder] ASC,
	[idProduct] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[DetailOrder]  WITH CHECK ADD  CONSTRAINT [FK_DetailOrder_Orders] FOREIGN KEY([idOrder])
REFERENCES [dbo].[Orders] ([idOrder])
GO
ALTER TABLE [dbo].[DetailOrder] CHECK CONSTRAINT [FK_DetailOrder_Orders]
GO
ALTER TABLE [dbo].[DetailOrder]  WITH CHECK ADD  CONSTRAINT [FK_DetailOrder_Products] FOREIGN KEY([idProduct])
REFERENCES [dbo].[Products] ([idProduct])
GO
ALTER TABLE [dbo].[DetailOrder] CHECK CONSTRAINT [FK_DetailOrder_Products]
GO



INSERT [dbo].[DetailOrder] ([idOrder], [idProduct], [quantity]) VALUES (1, 3, 1)
GO
INSERT [dbo].[DetailOrder] ([idOrder], [idProduct], [quantity]) VALUES (1, 5, 1)
GO
INSERT [dbo].[DetailOrder] ([idOrder], [idProduct], [quantity]) VALUES (2, 4, 1)
GO

--detailorder

