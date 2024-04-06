DROP procedure IF EXISTS `CountryList`;

DELIMITER $$
CREATE PROCEDURE `CountryList` ()
BEGIN
  SELECT
    countries.Name
  FROM
    countries;
END$$

DELIMITER ;

