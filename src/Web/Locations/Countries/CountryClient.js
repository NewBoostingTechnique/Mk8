import ApiClient from '../../App/ApiClient';

class CountryClient extends ApiClient {
  async listAsync() {
    const response = await super.fetchAsync('/api/country')
    return await response.json();
  }
}

const countryClient = new CountryClient();
export default function useCountryClient() {
  return countryClient;
}