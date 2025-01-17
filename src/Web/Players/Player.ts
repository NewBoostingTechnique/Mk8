export default class Player {
  constructor(
    public readonly name: string,
    public readonly active: Date | null,
    public readonly countryName: string,
    public readonly regionName: string
  ) { }
}
