import ApiClient from '../App/ApiClient';

class RegionClient extends ApiClient {
  async listAsync(countryName) {
    const response = await super.fetchAsync(`/api/regions/${countryName}/`);
    return await response.json();
  }
}

const regionClient = new RegionClient();
export default function useRegionClient() {
  return regionClient;
}
