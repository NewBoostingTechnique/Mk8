DROP procedure IF EXISTS `PersonIdentify`;

DELIMITER $$
CREATE PROCEDURE `PersonIdentify` (
  IN PersonName VARCHAR(255)
)
BEGIN
  SELECT
    persons.Id
  FROM
    persons
  WHERE
    persons.Name = PersonName;
END$$

DELIMITER ;
