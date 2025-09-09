USE UWBOX
GO

CREATE PROCEDURE SP_PARAM_PREMIUM_RATE_DETAIL_UPSERT
@CODE		varchar(30),
@AGE		int,
@TENOR_SEQ	int,
@RATE		float,
@MODE		varchar(2),
@USERBY	varchar(50)
AS

if not exists (select CODE from PARAM_PREMIUM_RATE_DETAIL where CODE = @CODE and AGE = @AGE and TENOR_SEQ = @TENOR_SEQ)
begin
	insert into PARAM_PREMIUM_RATE_DETAIL
	select
	CODE					= @CODE,
	AGE						= @AGE,
	TENOR_SEQ				= @TENOR_SEQ,
	RATE					= @RATE,
	RATE_FEMALE				= @RATE,
	RATE_SMOKER				= @RATE,
	RATE_FEMALE_SMOKER		= @RATE,
	CREATEBY				= @USERBY,
	CREATEDATE				= GETDATE(),
	LASTCHANGEBY			= @USERBY,
	LASTCHANGEDATE			= GETDATE()
end else
begin
	update PARAM_PREMIUM_RATE_DETAIL set	
	RATE					= (case when @MODE = 'M' then @RATE else RATE end),
	RATE_FEMALE				= (case when @MODE = 'F' then @RATE else RATE_FEMALE end),
	RATE_SMOKER				= (case when @MODE = 'MS' then @RATE else RATE_SMOKER end),
	RATE_FEMALE_SMOKER		= (case when @MODE = 'FS' then @RATE else RATE_FEMALE_SMOKER end),
	LASTCHANGEBY			= @USERBY,
	LASTCHANGEDATE			= GETDATE()
	where
	CODE					= @CODE
	and AGE					= @AGE
	and TENOR_SEQ			= @TENOR_SEQ
end
GO