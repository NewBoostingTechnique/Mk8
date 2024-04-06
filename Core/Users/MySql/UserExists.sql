DROP procedure IF EXISTS `UserExists`;

DELIMITER $$
CREATE PROCEDURE `UserExists` (
  IN UserEmail VARCHAR(32)
)
BEGIN
  SELECT
    1
  FROM
    users
  WHERE
    users.Email = UserEmail;
END$$

DELIMITER ;
