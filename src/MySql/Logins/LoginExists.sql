DROP procedure IF EXISTS `LoginExists`;

CREATE PROCEDURE `LoginExists` (
  IN LoginEmail VARCHAR(32)
)
BEGIN
  SELECT
    1
  FROM
    logins
  WHERE
    logins.Email = LoginEmail;
END;
