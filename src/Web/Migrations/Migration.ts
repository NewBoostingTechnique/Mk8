export default class Migration {
  constructor(
    public readonly id: string,
    public readonly startTime: string,
    public readonly description: string,
    public readonly progress: number
  ) { }
}
