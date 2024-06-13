DROP procedure IF EXISTS `CountryIdentify`;

CREATE PROCEDURE `CountryIdentify` (
  IN CountryName VARCHAR(255)
)
BEGIN
  SELECT
    countries.Id
  FROM
    countries
  WHERE
    countries.Name = CountryName;
END;
