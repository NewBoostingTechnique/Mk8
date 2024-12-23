import ApiClient from '../App/ApiClient';

class CourseClient extends ApiClient {

  static baseUri = '/api/course/';

  async indexAsync() {
    const response = await super.fetchAsync(CourseClient.baseUri);
    return await response.json();
  }
}

const courseClient = new CourseClient();
export default function useCourseClient() {
  return courseClient;
}
