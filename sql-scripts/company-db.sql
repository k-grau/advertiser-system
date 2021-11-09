CREATE TABLE [dbo].[Companies] (
    [CompanyId]        BIGINT          IDENTITY (1001, 1) NOT NULL,
    [OrgNo]   INT NOT NULL,
    [Name]   VARCHAR (30) NOT NULL,
    CONSTRAINT [PK_Tbl_Companies] PRIMARY KEY CLUSTERED ([CompanyId] ASC),
    CONSTRAINT [UQ_Tbl_Companies] UNIQUE([OrgNo]));
GO


CREATE TABLE [dbo].[Contact] (
    [ContactId] INT          IDENTITY (1, 1) NOT NULL,
    [ContactAddress]   VARCHAR (50) NOT NULL,
    [ContactPostcode]   INT NOT NULL,
    [ContactCity]   VARCHAR (50) NOT NULL,
    [ContactPhone]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Tbl_Contact] PRIMARY KEY CLUSTERED ([ContactId] ASC)
);
GO


CREATE TABLE [dbo].[Billing] (
    [BillingId] INT          IDENTITY (1, 1) NOT NULL,
    [BillingAddress]   VARCHAR (50) NOT NULL,
    [BillingPostcode]   INT NOT NULL,
    [BillingCity]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Tbl_Billing] PRIMARY KEY CLUSTERED ([BillingId] ASC)
);
GO



CREATE TABLE [dbo].[CompanyContact] (
    [CompanyId] INT NOT NULL,
    [ContactId]  INT NOT NULL,
    CONSTRAINT [UQ_Tbl_CompanyContact] UNIQUE NONCLUSTERED ([CompanyId] ASC, [ContactId] ASC),
    CONSTRAINT [FK1_Tbl_CompanyContact] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Companies] ([CompanyId])
    ON DELETE CASCADE
    ON UPDATE CASCADE,
    CONSTRAINT [FK2_Tbl_CompanyContact] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contact] ([ContactId])
);
GO


CREATE TABLE [dbo].[CompanyBilling] (
    [CompanyId] INT NOT NULL,
    [BillingId]  INT NOT NULL,
    CONSTRAINT [UQ_Tbl_CompanyBilling] UNIQUE NONCLUSTERED ([CompanyId] ASC, [BillingId] ASC),
    CONSTRAINT [FK1_Tbl_CompanyBilling] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Companies] ([CompanyId])
    ON DELETE CASCADE
    ON UPDATE CASCADE,
    CONSTRAINT [FK2_Tbl_CompanyBilling] FOREIGN KEY ([BillingId]) REFERENCES [dbo].[Billing] ([BillingId])
);
GO


CREATE OR ALTER PROC [dbo].[GetCompany]
@OrgNo INT,
@Name VARCHAR(30)
AS
BEGIN
DECLARE @LastId TABLE (CompanyId INT)
DECLARE @CompanyId INT
    BEGIN
        IF NOT EXISTS(SELECT Companies.CompanyId FROM Companies WHERE Companies.OrgNo = @OrgNo)
            BEGIN
                 INSERT INTO[dbo].[Companies] 
                ([OrgNo], [Name])
                OUTPUT INSERTED.CompanyId INTO @LastId
                VALUES
                (@OrgNo, @Name) 

                SET @CompanyId = (SELECT CompanyId FROM @LastId)                                                                            
        
            END 
        ELSE
        SET @CompanyId = (SELECT Companies.CompanyId FROM Companies WHERE Companies.OrgNo = @OrgNo);
    END
    SELECT Companies.CompanyId, Companies.OrgNo, Companies.Name 
        FROM Companies WHERE Companies.OrgNo = @OrgNo;
END
GO



CREATE OR ALTER PROC [dbo].[GetSetCompanyContact] 
    @CompanyId INT,
    @Address VARCHAR(30),
    @Postcode INT,
    @City VARCHAR(30),
    @Phone VARCHAR(50)
AS
BEGIN
    DECLARE @LastId TABLE (ContactId INT)
    DECLARE @ContactId INT
    BEGIN
        IF NOT EXISTS(SELECT Contact.ContactAddress, Contact.ContactPostcode, 
                Contact.ContactCity, Contact.ContactPhone
                FROM Contact
                WHERE Contact.ContactAddress = @Address AND Contact.ContactPostcode = @Postcode 
                AND Contact.ContactPhone = @Phone)
            BEGIN
                INSERT INTO[dbo].[Contact] 
                ([ContactAddress], [ContactPostcode], [ContactCity], [ContactPhone])
                OUTPUT INSERTED.ContactId INTO @LastId
                VALUES
                (@Address, @Postcode, @City, @Phone) 

                SET @ContactId = (SELECT ContactId FROM @LastId)

                INSERT INTO[dbo].[CompanyContact]
                ([CompanyId], [ContactId])
                VALUES
                (@CompanyId, @ContactId)
            END
        ELSE
            SET @ContactId = (SELECT Contact.ContactId FROM Contact WHERE Contact.ContactAddress = @Address)
            IF NOT EXISTS(SELECT CompanyContact.CompanyId FROM CompanyContact 
                    WHERE CompanyContact.CompanyId = @CompanyId AND CompanyContact.ContactId = @ContactId)
                BEGIN
                    INSERT INTO[dbo].[CompanyContact]
                    ([CompanyId], [ContactId])
                    VALUES
                    (@CompanyId, @ContactId)
                END
            
    END
    SELECT Companies.CompanyId, Contact.ContactId, Contact.ContactAddress, 
            Contact.ContactPostcode, Contact.ContactCity, Contact.ContactPhone
            FROM Contact
            JOIN CompanyContact ON Contact.ContactId = CompanyContact.ContactId
            JOIN Companies ON CompanyContact.CompanyId = Companies.CompanyId
            WHERE Contact.ContactId = @ContactId AND Companies.CompanyId = @CompanyId 
END
GO



CREATE OR ALTER PROC [dbo].[InsertCompanyContact] 
    @CompanyId INT,
    @Address VARCHAR(30),
    @Postcode INT,
    @City VARCHAR(30),
    @Phone VARCHAR(50)
AS
BEGIN
    DECLARE @LastId TABLE (ContactId INT)
    DECLARE @ContactId INT
    BEGIN
        IF NOT EXISTS(SELECT Contact.ContactAddress, Contact.ContactPostcode, 
                Contact.ContactCity, Contact.ContactPhone
                FROM Contact
                WHERE Contact.ContactAddress = @Address AND Contact.ContactPostcode = @Postcode 
                AND Contact.ContactPhone = @Phone)
            BEGIN
                INSERT INTO[dbo].[Contact] 
                ([ContactAddress], [ContactPostcode], [ContactCity], [ContactPhone])
                OUTPUT INSERTED.ContactId INTO @LastId
                VALUES
                (@Address, @Postcode, @City, @Phone) 

                SET @ContactId = (SELECT ContactId FROM @LastId)

                INSERT INTO[dbo].[CompanyContact]
                ([CompanyId], [ContactId])
                VALUES
                (@CompanyId, @ContactId)
            END
        ELSE
            SET @ContactId = (SELECT Contact.ContactId FROM Contact WHERE Contact.ContactAddress = @Address)
            IF NOT EXISTS(SELECT CompanyContact.CompanyId FROM CompanyContact 
                    WHERE CompanyContact.CompanyId = @CompanyId AND CompanyContact.ContactId = @ContactId)
                BEGIN
                    INSERT INTO[dbo].[CompanyContact]
                    ([CompanyId], [ContactId])
                    VALUES
                    (@CompanyId, @ContactId)
                END
            
    END
END
GO



CREATE OR ALTER PROC [dbo].[GetSetCompanyBilling] 
    @CompanyId INT,
    @Address VARCHAR(30),
    @Postcode INT,
    @City VARCHAR(30)
AS
BEGIN
    DECLARE @LastId TABLE (BillingId INT)
    DECLARE @BillingId INT
    BEGIN
        IF NOT EXISTS(SELECT Billing.BillingAddress, Billing.BillingPostcode, 
                Billing.BillingCity 
                FROM Billing
                WHERE Billing.BillingAddress = @Address AND Billing.BillingPostcode = @Postcode
                AND Billing.BillingCity = @City)
            BEGIN
                INSERT INTO[dbo].[Billing] 
                ([BillingAddress], [BillingPostcode], [BillingCity])
                OUTPUT INSERTED.BillingId INTO @LastId
                VALUES
                (@Address, @Postcode, @City) 

                SET @BillingId = (SELECT BillingId FROM @LastId)

                INSERT INTO[dbo].[CompanyBilling]
                ([CompanyId], [BillingId])
                VALUES
                (@CompanyId, @BillingId)
            END
        ELSE
            SET @BillingId = (SELECT Billing.BillingId FROM Billing WHERE Billing.BillingAddress = @Address)
            IF NOT EXISTS(SELECT CompanyBilling.CompanyId FROM CompanyBilling 
                    WHERE CompanyBilling.CompanyId = @CompanyId AND CompanyBilling.BillingId = @BillingId)
                BEGIN
                    INSERT INTO[dbo].[CompanyBilling]
                    ([CompanyId], [BillingId])
                    VALUES
                    (@CompanyId, @BillingId)
                END
            
        END
    SELECT Companies.CompanyId, Billing.BillingId, Billing.BillingAddress, 
            Billing.BillingPostcode, Billing.BillingCity
            FROM Billing
            JOIN CompanyBilling ON Billing.BillingId = CompanyBilling.BillingId
            JOIN Companies ON CompanyBilling.CompanyId = Companies.CompanyId
            WHERE Billing.BillingId = @BillingId AND Companies.CompanyId = @CompanyId 
END
GO



CREATE OR ALTER PROC [dbo].[InsertCompanyBilling] 
    @CompanyId INT,
    @Address VARCHAR(30),
    @Postcode INT,
    @City VARCHAR(30)
AS
BEGIN
    DECLARE @LastId TABLE (BillingId INT)
    DECLARE @BillingId INT
    BEGIN
        IF NOT EXISTS(SELECT Billing.BillingAddress, Billing.BillingPostcode, 
                Billing.BillingCity 
                FROM Billing
                WHERE Billing.BillingAddress = @Address AND Billing.BillingPostcode = @Postcode
                AND Billing.BillingCity = @City)
            BEGIN
                INSERT INTO[dbo].[Billing] 
                ([BillingAddress], [BillingPostcode], [BillingCity])
                OUTPUT INSERTED.BillingId INTO @LastId
                VALUES
                (@Address, @Postcode, @City) 

                SET @BillingId = (SELECT BillingId FROM @LastId)

                INSERT INTO[dbo].[CompanyBilling]
                ([CompanyId], [BillingId])
                VALUES
                (@CompanyId, @BillingId)
            END
        ELSE
            SET @BillingId = (SELECT Billing.BillingId FROM Billing WHERE Billing.BillingAddress = @Address)
            IF NOT EXISTS(SELECT CompanyBilling.CompanyId FROM CompanyBilling 
                    WHERE CompanyBilling.CompanyId = @CompanyId AND CompanyBilling.BillingId = @BillingId)
                BEGIN
                    INSERT INTO[dbo].[CompanyBilling]
                    ([CompanyId], [BillingId])
                    VALUES
                    (@CompanyId, @BillingId)
                END
            
    END
END
GO



CREATE OR ALTER PROC [dbo].[GetFullCompanyDetails]
@CompanyId INT,
@BillingId INT,
@ContactId INT
AS
BEGIN
    SELECT Companies.CompanyId, Companies.OrgNo, Companies.Name, 
    Contact.ContactId, Contact.ContactAddress, Contact.ContactPostcode, Contact.ContactCity, Contact.ContactPhone,
    Billing.BillingId, Billing.BillingAddress, Billing.BillingPostcode, Billing.BillingCity
    FROM Companies, Contact, Billing
    WHERE Companies.CompanyId = @CompanyId AND Contact.ContactId = @ContactId AND Billing.BillingId = @BillingId 
END
GO



CREATE OR ALTER PROC [dbo].[GetCompanyContact]
@CompanyId INT,
@ContactId INT
AS
BEGIN
    SELECT Contact.ContactId, Contact.ContactAddress, Contact.ContactPostcode, Contact.ContactCity, Contact.ContactPhone
    FROM Companies, Contact
    WHERE Companies.CompanyId = @CompanyId AND Contact.ContactId = @ContactId;
END
GO



CREATE OR ALTER PROC [dbo].[GetCompanyBilling]
@CompanyId INT,
@BillingId INT
AS
BEGIN
    SELECT Billing.BillingId, Billing.BillingAddress, Billing.BillingPostcode, Billing.BillingCity
    FROM Companies, Billing
    WHERE Companies.CompanyId = @CompanyId AND Billing.BillingId = @BillingId;
END
GO

