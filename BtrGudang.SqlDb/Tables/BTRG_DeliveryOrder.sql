CREATE TABLE [dbo].[BTRG_DeliveryOrder]
(
	DeliveryOrderId VARCHAR(26) NOT NULL CONSTRAINT DF_BTRG_DeliveryOrder_DeliveryOrderId DEFAULT (''),
	DeliveryOrderDate DATETIME NOT NULL CONSTRAINT DF_BTRG_DeliveryOrder_DeliveryOrderDate DEFAULT ('3000-01-01'),
	DeliveryOrderCode VARCHAR(20) NOT NULL CONSTRAINT DF_BTRG_DeliveryOrder_DeliveryOrderCode DEFAULT (''),

	CustomerId VARCHAR(26) NOT NULL CONSTRAINT DF_BTRG_DeliveryOrder_CustomerId DEFAULT (''),
	CustomerCode VARCHAR(10) NOT NULL CONSTRAINT DF_BTRG_DeliveryOrder_CustomerCode DEFAULT (''),
	CustomerName VARCHAR(100) NOT NULL CONSTRAINT DF_BTRG_DeliveryOrder_CustomerName DEFAULT (''),
	Alamat VARCHAR(200) NOT NULL CONSTRAINT DF_BTRG_DeliveryOrder_AlamatKirim DEFAULT (''),
	NoTelp VARCHAR(20) NOT NULL CONSTRAINT DF_BTRG_DeliveryOrder_NoTelp DEFAULT (''),

	FakturId VARCHAR(26) NOT NULL CONSTRAINT DF_BTRG_DeliveryOrder_FakturId DEFAULT (''),
	FakturCode VARCHAR(26) NOT NULL CONSTRAINT DF_BTRG_DeliveryOrder_FakturCode DEFAULT (''),
	FakturDate DATETIME NOT NULL CONSTRAINT DF_BTRG_DeliveryOrder_FakturDate DEFAULT ('3000-01-01'),
	AdminName VARCHAR(100) NOT NULL CONSTRAINT DF_BTRG_DeliveryOrder_AdminName DEFAULT (''),

	Latitude DECIMAL(18, 15) NOT NULL CONSTRAINT DF_BTRG_DeliveryOrder_Latitude DEFAULT (0),
	Longitude DECIMAL(18, 15) NOT NULL CONSTRAINT DF_BTRG_DeliveryOrder_Longitude DEFAULT (0),
	Accuracy INT NOT NULL CONSTRAINT DF_BTRG_DeliveryOrder_Accuracy DEFAULT (0),

	CONSTRAINT PK_BTRG_DeliveryOrder PRIMARY KEY CLUSTERED (DeliveryOrderId)
)
