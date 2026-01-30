CREATE TABLE [dbo].[BTRG_DeliveryOrderItem]
(
	DeliveryOrderId VARCHAR(26) NOT NULL CONSTRAINT DF_BTRG_DeliveryOrderItem_DeliveryOrderId DEFAULT (''),
	NoUrut INT NOT NULL CONSTRAINT DF_BTRG_DeliveryOrderItem_NoUrut DEFAULT (0),
	BrgId VARCHAR(6) NOT NULL CONSTRAINT DF_BTRG_DeliveryOrderItem_BrgId DEFAULT (''),
	BrgCode VARCHAR(20) NOT NULL CONSTRAINT DF_BTRG_DeliveryOrderItem_BrgCode DEFAULT (''),
	BrgName VARCHAR(100) NOT NULL CONSTRAINT DF_BTRG_DeliveryOrderItem_BrgName DEFAULT (''),
	QtyBesar INT NOT NULL CONSTRAINT DF_BTRG_DeliveryOrderItem_QtyBesar DEFAULT (0),
	SatBesar VARCHAR(10) NOT NULL CONSTRAINT DF_BTRG_DeliveryOrderItem_SatBesar DEFAULT (''),
	QtyKecil INT NOT NULL CONSTRAINT DF_BTRG_DeliveryOrderItem_QtyKecil DEFAULT (0),
	SatKecil VARCHAR(10) NOT NULL CONSTRAINT DF_BTRG_DeliveryOrderItem_SatKecil DEFAULT (''),

	CONSTRAINT PK_BTRG_DeliveryOrderItem PRIMARY KEY CLUSTERED (DeliveryOrderId, NoUrut)
)
