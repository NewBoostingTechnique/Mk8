export default class ProofType {
  static NoProof = new ProofType('No Proof');
  static Screenshot = new ProofType('Screenshot');
  static TimeScroll = new ProofType('Time Scroll');
  static Video = new ProofType('Video');
  static Ghost = new ProofType('Ghost');

  constructor(description) {
    this.description = description;
  }
}