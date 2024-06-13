DROP procedure IF EXISTS `CountryList`;

CREATE PROCEDURE `CountryList` ()
BEGIN
  SELECT
    countries.Name
  FROM
    countries;
END;
