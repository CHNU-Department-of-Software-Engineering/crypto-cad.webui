const populateEditedFieldToConfig = (configuration) => {
  const parsedConfiguration = configuration ? JSON.parse(configuration) : null

  return Object.keys(parsedConfiguration).reduce((result, configurationItemKey) => {
    return {
      ...result,
      [configurationItemKey]: {
        data: parsedConfiguration[configurationItemKey],
        edited: false
      }
    }
  }, {})
}

const getOnlyEditedPartOfConfig = (configuration) => Object.keys(configuration).reduce(
  (result, configItemKey) => {
    if (configuration[configItemKey].edited) {
      return {
        ...result,
        [configItemKey]: configuration[configItemKey].data
      }
    }

    return result
  }, {}
)

export {
  populateEditedFieldToConfig,
  getOnlyEditedPartOfConfig
}
