import ApiClient from '../App/ApiClient';

class CountryClient extends ApiClient {
  async indexAsync() {
    const response = await super.fetchAsync('/api/countries/')
    return await response.json();
  }
}

const countryClient = new CountryClient();
export default function useCountryClient() {
  return countryClient;
}
