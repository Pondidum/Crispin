const defaultState = {
  loading: false,
  toggles: []
};

export default (state = defaultState, action) => {
  switch (action.type) {
    case "LIST_TOGGLES_REQUEST":
      return {
        loading: true,
        toggles: []
      };

    case "LIST_TOGGLES_SUCCESS":
      return {
        loading: false,
        toggles: action.toggles
      };

    case "CREATE_TOGGLE_SUCCESS":
      return Object.assign({}, state, {
        toggles: state.toggles.concat([action.toggle])
      });

    default:
      return state;
  }
};
