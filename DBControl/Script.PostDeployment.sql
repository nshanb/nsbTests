/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

if(not exists (select top 1 * from sync4partner.mainconfig))
insert sync4partner.mainconfig VALUES('1879-03-19')
if(not exists (select top 1 * from sync4partner.taskconfig))
insert sync4partner.taskconfig (Partnerid) VALUES(-1)

