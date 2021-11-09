CREATE TABLE [dbo].[Subscribers] (
    [SubscriberNo]        BIGINT          IDENTITY (1001, 1) NOT NULL,
    [PersonalNo]   INT NOT NULL,
    [Firstname]   VARCHAR (30) NOT NULL,
	[Lastname]   VARCHAR (30) NOT NULL,
    CONSTRAINT [PK_Tbl_Subscribers] PRIMARY KEY CLUSTERED ([SubscriberNo] ASC));
GO


CREATE TABLE [dbo].[SubscriberContact] (
    [SubscriberNo] INT NOT NULL,
    [Address]   VARCHAR (50) NOT NULL,
    [Postcode]   INT NOT NULL,
    [City]   VARCHAR (50) NOT NULL,
    [Phone]   VARCHAR (50) NOT NULL);
GO


ALTER TABLE [SubscriberContact]
ADD CONSTRAINT [FK_Tbl_SubscriberContact] FOREIGN KEY(SubscriberNo) 
    REFERENCES  Subscribers(SubscriberNo)
    ON DELETE CASCADE
    ON UPDATE CASCADE
GO


ALTER TABLE [SubscriberContact]
    ADD CONSTRAINT [PK_Tbl_SubscriberContact] PRIMARY KEY CLUSTERED ([SubscriberNo] ASC)
GO



CREATE OR ALTER PROC [dbo].[UpdateSubscribers] 
    @SubscriberNo INT,
    @PersonalNo BIGINT=NULL,
    @Firstname VARCHAR(30)=NULL,
    @Lastname VARCHAR(30)=NULL,
    @Address VARCHAR(50)=NULL,
    @Postcode INT = NULL,
    @City VARCHAR(50)=NULL,
    @Phone VARCHAR(50)=NULL
AS
    BEGIN
    BEGIN TRANSACTION
        UPDATE dbo.Subscribers
            SET PersonalNo=ISNULL(@PersonalNo, PersonalNo),
                FirstName=ISNULL(@Firstname, Firstname),
                Lastname=ISNULL(@Lastname, Lastname)
        WHERE SubscriberNo = @SubscriberNo;
        UPDATE dbo.SubscriberContact
            SET Address=ISNULL(@Address, Address),
                Postcode=ISNULL(@Postcode, Postcode),
                City=ISNULL(@City, City),
                Phone=ISNULL(@Phone, Phone)
        WHERE SubscriberNo = @SubscriberNo
    COMMIT;
END
GO




CREATE OR ALTER PROC [dbo].[GetSubscriber]
@SubscriberNo INT
AS
BEGIN
    IF EXISTS(SELECT Subscribers.SubscriberNo FROM Subscribers WHERE Subscribers.SubscriberNo = @SubscriberNo)
        BEGIN
            SELECT Subscribers.SubscriberNo, Subscribers.PersonalNo, Subscribers.Firstname, Subscribers.Lastname, SubscriberContact.Address,
                SubscriberContact.Postcode, SubscriberContact.City, SubscriberContact.Phone
                FROM Subscribers, SubscriberContact
            WHERE Subscribers.SubscriberNo = @SubscriberNo AND SubscriberContact.SubscriberNo = @SubscriberNo
        END
    ELSE
        RAISERROR('Det finns inget sådant prenumerantnummer i databasen!', 1, 1);
END
GO
      



CREATE OR ALTER PROC [dbo].[GetSubscribers]
AS
BEGIN
    BEGIN
        SELECT Subscribers.SubscriberNo, Subscribers.PersonalNo, Subscribers.Firstname, Subscribers.Lastname, SubscriberContact.Address,
            SubscriberContact.Postcode, SubscriberContact.City, SubscriberContact.Phone
            FROM Subscribers
        INNER JOIN SubscriberContact ON Subscribers.SubscriberNo=SubscriberContact.SubscriberNo 
    END
END
GO



