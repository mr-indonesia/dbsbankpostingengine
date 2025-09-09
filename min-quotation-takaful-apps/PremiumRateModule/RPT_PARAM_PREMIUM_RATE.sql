USE UWBOX
GO

CREATE PROCEDURE [dbo].[RPT_PARAM_PREMIUM_RATE]
@CODE	varchar(30)
AS

if exists (select CODE from PARAM_PREMIUM_RATE_MASTER where CODE = @CODE and TENOR_CODE <> 'HP')
begin
	if exists (select CODE from PARAM_PREMIUM_RATE_DETAIL where CODE = @CODE)
	begin
		select
		MODE_SEQ		= 1,
		MODE			= 'MALE',
		AGE				= AGE,
		TENOR_SEQ		= '# ' + convert(varchar(10), TENOR_SEQ),
		RATE			= isnull(RATE, 0),
		SEQ				= TENOR_SEQ
		from			PARAM_PREMIUM_RATE_DETAIL
		where
		CODE = @CODE

		union all

		select
		MODE_SEQ		= 2,
		MODE			= 'FEMALE',
		AGE				= AGE,
		TENOR_SEQ		= '# ' + convert(varchar(10), TENOR_SEQ),
		RATE			= isnull(RATE_FEMALE, 0),
		SEQ				= TENOR_SEQ
		from			PARAM_PREMIUM_RATE_DETAIL
		where
		CODE = @CODE

		union all

		select
		MODE_SEQ		= 3,
		MODE			= 'MALE-SMOKER',
		AGE				= AGE,
		TENOR_SEQ		= '# ' + convert(varchar(10), TENOR_SEQ),
		RATE			= isnull(RATE_SMOKER, 0),
		SEQ				= TENOR_SEQ
		from			PARAM_PREMIUM_RATE_DETAIL
		where
		CODE = @CODE

		union all

		select
		MODE_SEQ		= 4,
		MODE			= 'FEMALE-SMOKER',
		AGE				= AGE,
		TENOR_SEQ		= '# ' + convert(varchar(10), TENOR_SEQ),
		RATE			= isnull(RATE_FEMALE_SMOKER, 0),
		SEQ				= TENOR_SEQ
		from			PARAM_PREMIUM_RATE_DETAIL
		where
		CODE = @CODE
	end else
	begin
		select
		MODE_SEQ		= 1,
		MODE			= 'MALE',
		AGE				= a.SEQ,
		TENOR_SEQ		= '# ' + convert(varchar(10), b.SEQ),
		RATE			= 0,
		SEQ				= b.SEQ
		from			SC_SEQ a
		inner join		SC_SEQ b on b.SEQ between 1 and 80
		where
		a.SEQ between 0 and 81

		union all

		select
		MODE_SEQ		= 2,
		MODE			= 'FEMALE',
		AGE				= a.SEQ,
		TENOR_SEQ		= '# ' + convert(varchar(10), b.SEQ),
		RATE			= 0,
		SEQ				= b.SEQ
		from			SC_SEQ a
		inner join		SC_SEQ b on b.SEQ between 1 and 80
		where
		a.SEQ between 0 and 81

		union all

		select
		MODE_SEQ		= 3,
		MODE			= 'MALE-SMOKER',
		AGE				= a.SEQ,
		TENOR_SEQ		= '# ' + convert(varchar(10), b.SEQ),
		RATE			= 0,
		SEQ				= b.SEQ
		from			SC_SEQ a
		inner join		SC_SEQ b on b.SEQ between 1 and 80
		where
		a.SEQ between 0 and 81

		union all

		select
		MODE_SEQ		= 4,
		MODE			= 'FEMALE-SMOKER',
		AGE				= a.SEQ,
		TENOR_SEQ		= '# ' + convert(varchar(10), b.SEQ),
		RATE			= 0,
		SEQ				= b.SEQ
		from			SC_SEQ a
		inner join		SC_SEQ b on b.SEQ between 1 and 80
		where
		a.SEQ between 0 and 81
	end
end else
begin

	if exists (select CODE from PARAM_PREMIUM_RATE_DETAIL_HEALTH where CODE = @CODE)
	begin
		select
		MODE_SEQ		= b.SEQ,
		MODE			= a.BENEFIT_ID,
		AGE				= a.AGE,
		TENOR_SEQ		= a.PLAN_VALUE,
		RATE			= isnull(a.RATE, 0),
		SEQ				= a.PLAN_VALUE
		from			PARAM_PREMIUM_RATE_DETAIL_HEALTH a
		inner join		PARAM_HEALTH_BENEFIT b on a.BENEFIT_ID = b.BENEFIT_ID
		where
		a.CODE = @CODE
	end else
	begin
		select
		MODE_SEQ		= a.SEQ,
		MODE			= a.BENEFIT_ID,
		AGE				= b.SEQ,
		TENOR_SEQ		= 100,
		RATE			= 0,
		SEQ				= b.SEQ
		from			PARAM_HEALTH_BENEFIT a
		inner join		SC_SEQ b on b.SEQ < 81
		order by 
		b.SEQ
	end

end