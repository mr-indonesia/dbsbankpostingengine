USE UWBOX
GO

CREATE PROCEDURE [dbo].[SP_PARAM_PREMIUM_RATE_DETAIL]
@CODE varchar(30),
@SEX char(10)
AS


DECLARE @cols NVARCHAR(MAX), @query NVARCHAR(MAX)

SET @cols = STUFF(
                 (
                     SELECT 
                     ','+QUOTENAME(a.TENOR_SEQ)
                     FROM PARAM_PREMIUM_RATE_DETAIL a 
					 where
					 a.CODE = @CODE
					 group by a.TENOR_SEQ
					 order by a.TENOR_SEQ 
					 FOR XML PATH(''), TYPE
                 ).value('.', 'nvarchar(max)'), 1, 1, '');

--SET @query =	'SELECT AGE, ' +
--				@cols +
--				'from (	SELECT AGE, TENOR_SEQ, RATE
--						FROM PARAM_PREMIUM_RATE_DETAIL
--						where
--						CODE = ''' + @CODE + '''
--				)x pivot (AVG(RATE) for TENOR_SEQ in ('+@cols+')) p ' +
--				'order by AGE'


DECLARE @FIELDNAME varchar(50)
select 
@FIELDNAME = (case	when @SEX = 'M' then 'RATE' 
					when @SEX = 'F' then 'RATE_FEMALE'
					when @SEX = 'MS' then 'RATE_SMOKER' 
					when @SEX = 'FS' then 'RATE_FEMALE_SMOKER' 
					else '' end)


SET @query =
	'SELECT AGE, ' +
				@cols +
				'from (	SELECT AGE, TENOR_SEQ, ' + @FIELDNAME + 
				'		FROM PARAM_PREMIUM_RATE_DETAIL
						where
						CODE = ''' + @CODE + '''
				)x pivot (AVG(' +@FIELDNAME+ ') for TENOR_SEQ in ('+@cols+')) p ' +
				'order by AGE'

EXECUTE (@query)
GO