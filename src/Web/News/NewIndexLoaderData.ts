import New from "./New";

export default class NewIndexLoaderData {
  constructor(
    public readonly authorization: boolean,
    public readonly news: New[]
  ) { }
}
