use master

select *from Users
select *from League
select * from Players
select * from Clubs

Create Database FLMDB

use FLMDB

set dateformat dmy

/*1*/
Create Table League
(
   ID int identity(1,1),
   MaLeague as 'League' + right('000' + cast (ID as varchar(3)), 3) persisted,
   LeagueName Nvarchar(15) unique not null,
   MaxTeams int not null,
   StartDate smalldatetime not null,
   EndDate smalldatetime not null,
   constraint PK_MaLeague primary key(MaLeague)
)

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

insert into Users(Username, Password, Email, LoaiUsers) values ('admin', 'Admin123', 'admin123@gmail.com', 0)

Create Table Clubs
(
    ID int identity(1,1),
	MaCLB as 'Club' + right('000' + cast (ID as varchar(3)), 3) persisted,
	TenCLB Nvarchar(20) not null,
	DiaChi Nvarchar(40) not null,
	TenSVD Nvarchar(20) not null,
	constraint PK_MaCLB primary key(MaCLB)
)
alter table Clubs add Img_File nvarchar(150)

insert into Clubs values('Manchester United', 'Manchester', 'Old Trafford', 'Club003.png')

/*alter table Clubs drop FK_MaCT 1
alter table Clubs drop column MaCT 2*/


Create Table Players
(
    /*Nên nghĩ cách thiết lập mã CT cho dễ phân biệt hơn: Real-Player0001, MU-Player0001*/
    ID int identity(1,1),
	MaCT as 'Player' + right('000' + cast (ID as varchar(3)), 3) persisted,
	TenCT Nvarchar(30) not null,
	LoaiCT int not null, /*0: Nội binh | 1: Ngoại binh*/
	QuocTich Nvarchar(30),
	ChieuCao int,
	CanNang int,
	ViTriChinh Nvarchar(30),
	ViTriPhu Nvarchar(30),
	NgaySinh date,
	SoAo int,
	ChanThuan Nvarchar(30),
	MaCLB varchar(7) not null,
	Luong int,
	constraint PK_MaCT primary key(MaCT)
)
/*alter table Players drop FK_MaCLB 1
alter table Players drop column MaCLB*/

/*xài cách này nếu gặp lỗi IDENTITY_INSERT is ON*/
SET IDENTITY_INSERT Players ON
INSERT INTO Players ([TenCT], [LoaiCT], [QuocTich], [ChieuCao], [CanNang], [ViTriChinh], [ViTriPhu], [NgaySinh], [SoAo], [ChanThuan], [MaCLB],[Luong])
VALUES ('chin', 1, 'su', 190, 09, 'q', 'w', '15-2-1997', 27, 'Phai', 'Club001', 12000000)
SET IDENTITY_INSERT Players OFF

/*Còn không thì thêm bình thường như này*/
insert into Players values(N'Rashford', 0, 'Anh', 180, 70, 'LW', 'CF', '1997-10-31', 10, 'Chan Phai', 'Club001',2000000)

/*Alter table Players add constraint FK_MACLB foreign key(MaCLB) references Clubs(MaCLB)
Alter table Clubs add constraint FK_MACT foreign key(MaCT) references Players(MaCT)*/

Create table Matchs
(
    ID int identity(1,1),
	MaTD as 'Match' + right('000' + cast (ID as varchar(3)), 3) persisted,
	MaDoiNha varchar(7) not null, /*ref MaCLB*/
	MaDoiKhach varchar(7) not null, /*ref MaCLB*/
	SoBanThangDoiNha int,
	SoBanThangDoiKhach int,
	constraint PK_MaTD primary key(MaTD)
)
Alter table Matchs add constraint FK_MADOINHA foreign key(MaDoiNha) references Clubs(MaCLB)
Alter table Matchs add constraint FK_MADOIKHACH foreign key(MaDoiKhach) references Clubs(MaCLB)

Create table Goals
(
    ID int identity(1,1),
	MaBT as 'Goal' + right('000' + cast (ID as varchar(3)), 3) persisted,
	LoaiBT nvarchar(30), /*Phản lưới nhà, phạt đền, solo, ...*/
	Phut int not null,
	PhutBuGio int,
	MaCTGhiBan varchar(9) not null,
	MaCTKienTao varchar(9) not null,
	MaTD varchar(8) not null,
    constraint PK_MaBT primary key(MaBT),
)
Alter table Goals add constraint FK_MATD foreign key(MaTD) references Matchs(MaTD)
Alter table Goals add constraint Check_Phut check (Phut>=0 and Phut <=90)

Go Create trigger Goals_PhutBuGio /*Chạy Go trước rồi chạy triggers sau*/
on Goals
after insert, update
as
begin
	declare @phut int
	declare @phutbugio int
	select @phut=Phut, @phutbugio=PhutBuGio from inserted
	if (@phut not in (45, 90) and @phutbugio>=0)
	begin
		print('Phút bù giờ chỉ được gán khi phút chính thức là 45 hoặc 90')
		rollback tran
	end
end

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
	SoTheVangDoiNha int,
	SoTheVangDoiKhach int,
	SoTheDoDoiNha int,
	SoTheDoDoiKhach int,
	SoPhatGocDoiNha int,
	SoPhatGocDoiKhach int,
)
Alter table MatchDetails add constraint FK_MATD_CTTD foreign key(MaTD) references Matchs(MaTD)

Create table Cards
(
    ID int identity(1,1),
	MaThe as 'Card' + right('000' + cast (ID as varchar(3)), 3) persisted,
	LoaiThe int, ---1: Thẻ vàng, 2: Thẻ đỏ---
	Phut int not null,
	PhutBuGio int,
	MaCTNhanThe varchar(9) not null,
	MaTD varchar(8) not null,
    constraint PK_MaThe primary key(MaThe),
)

Go Create trigger Cards_PhutBuGio 
on Cards	
after insert, update
as
begin
	declare @phut int
	declare @phutbugio int
	select @phut=Phut, @phutbugio=PhutBuGio from inserted
	if (@phut not in (45, 90) and @phutbugio>=0)
	begin
		print('Phút bù giờ chỉ được gán khi phút chính thức là 45 hoặc 90')
		rollback tran
	end
end

Alter table Cards add constraint FK_MACTNhanThe foreign key(MaCTNhanThe) references Players(MaCT)
