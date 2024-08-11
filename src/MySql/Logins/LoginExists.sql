DROP procedure IF EXISTS `LoginExists`;

CREATE PROCEDURE `LoginExists` (
  IN Email VARCHAR(255)
)
BEGIN
  SELECT
    1
  FROM
    logins
  WHERE
    logins.Email = Email;
END;
