let localeName: string | null = null;

export default async function getLocaleNameAsync() {
  if (localeName)
    return localeName;

  const candidates = navigator.languages.map(language => language.toLowerCase());

  for (const candidate of candidates) {
    try {
      await import(`dayjs/locale/${candidate}`);
      localeName = candidate;
      return localeName;
    }
    catch {
      console.info(`Locale ${candidate} is not supported.`);
    }
  }

  localeName = 'en';
  return localeName;
}
