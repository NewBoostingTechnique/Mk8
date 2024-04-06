import ApiClient from '../App/ApiClient';

class ProofTypeClient extends ApiClient {
  async listAsync() {
    const response = await super.fetchAsync('/api/proofType')
    return await response.json();
  }
}

const proofTypeClient = new ProofTypeClient();
export default function useProofTypeClient() {
  return proofTypeClient;
}