use master

Create Database FLMDB

use FLMDB
set dateformat dmy

Create Table Users
(
   ID int identity(1,1),
   MaUsers as 'User' + right('000' + cast (ID as varchar(3)), 3) persisted,
   UserName Nvarchar(15) unique not null,
   Password Nvarchar(20) not null,
   SDT char(15),
   Email NVarchar(30) not null,   /*Email để khôi phục mật khẩu nếu cần*/
   LoaiUsers int not null  /* 0: Organizers | 1: Clubs*/
   constraint PK_MaUsers primary key(MaUsers)
)

Create Table Clubs
(
    ID int identity(1,1),
	MaCLB as 'Club' + right('000' + cast (ID as varchar(3)), 3) persisted,
	TenCLB Nvarchar(20) not null,
	DiaChi Nvarchar(40) not null,
	TenSVD Nvarchar(20) not null,
	constraint PK_MaCLB primary key(MaCLB),
	MaCT varchar(9) not null
    /*Khoá ngoại tham chiếu Mã Cầu Thủ*/
)

Create Table Players
(
    /*Nên nghĩ cách thiết lập mã CT cho dễ phân biệt hơn: Real-Player0001, MU-Player0001*/
    ID int identity(1,1),
	MaCT as 'Player' + right('000' + cast (ID as varchar(3)), 3) persisted,
	TenCT Nvarchar(30) not null,
	LoaiCT int not null, /*0: Nội binh | 1: Ngoại binh*/
	Luong money,
	ChieuCao int,
	CanNang int,
	ViTriChinh Nvarchar(30),
	ViTriPhu Nvarchar(30),
	NgaySinh smalldatetime,
	SoAo int,
	ChanThuan Nvarchar(30),
	MaCLB varchar(7) not null,
	constraint PK_MaCT primary key(MaCT)
)
Alter table Players add constraint FK_MACLB foreign key(MaCLB) references Clubs(MaCLB)
Alter table Clubs add constraint FK_MACT foreign key(MaCT) references Players(MaCT)

Create table Matchs
(
    ID int identity(1,1),
	MaTD as 'Match' + right('000' + cast (ID as varchar(3)), 3) persisted,
	MaDoiNha varchar(7) not null, /*ref MaCLB*/
	MaDoiKhach varchar(7) not null, /*ref MaCLB*/
	SoBanThangDoiNha int,
	SoBanThangDoiKhach int,
	SoTheVangDoiNha int,
	SoTheVangDoiKhach int,
	SoTheDoDoiNha int,
	SoTheDoDoiKhach int,
	constraint PK_MaTD primary key(MaTD)
)
Alter table Matchs add constraint FK_MADOINHA foreign key(MaDoiNha) references Clubs(MaCLB)
Alter table Matchs add constraint FK_MADOIKHACH foreign key(MaDoiKhach) references Clubs(MaCLB)

Create table Goals
(
    ID int identity(1,1),
	MaBT as 'Goal' + right('000' + cast (ID as varchar(3)), 3) persisted,
	LoaiBT nvarchar(30), /*Phản lưới nhà, phạt đền, solo, ...*/
	ThoiDiemGhiBan smalldatetime,
	MaCTGhiBan varchar(9) not null,
	MaCTKienTao varchar(9) not null,
	MaTD varchar(8) not null,
    constraint PK_MaBT primary key(MaBT),
)
Alter table Goals add constraint FK_MATD foreign key(MaTD) references Matchs(MaTD)

Create table MatchDetails
(
    ID int identity(1,1),
	MaCTTD as 'MDT' + right('000' + cast (ID as varchar(3)), 3) persisted, /*6*/
	MaTD varchar(8) not null, /*ref*/
	KiemSoatBongDoiNha int, /*%*/
	KiemSoatBongDoiKhach int,
	SoCuSutDoiNha int,
	SoCuSutDoiKhach int,
	SoDuongChuyenDoiNha int,
	SoDuongChuyenDoiKhach int,
	SoLanPhamLoiDoiNha int,
	SoLanPhamLoiDoiKhach int,
	MaCTNhanTheVang varchar(9),
	MaCTNhanTheDo varchar(9),
)
Alter table MatchDetails add constraint FK_MATD_CTTD foreign key(MaTD) references Matchs(MaTD)
Alter table MatchDetails add constraint FK_MACTTV foreign key(MaCTNhanTheVang) references Players(MaCT)
Alter table MatchDetails add constraint FK_MACTTD foreign key(MaCTNhanTheDo) references Players(MaCT)