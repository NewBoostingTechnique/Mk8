DROP procedure IF EXISTS `LoginExists`;

CREATE PROCEDURE `LoginExists` (
  IN LoginEmail BINARY(16)
)
BEGIN
  SELECT
    1
  FROM
    logins
  WHERE
    logins.Email = LoginEmail;
END;
