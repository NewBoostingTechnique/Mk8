export default class ApiError extends Error {
  constructor(
    message: string,
    public readonly traceId: string
  ) {
    super(message);
  }
}
