import { ALL_TOGGLES_REQUESTED, ALL_TOGGLES_RECEIVED } from "./actions";

const DefaultState = {
  isFetching: false,
  all: []
};

const reducer = (state = DefaultState, action) => {
  switch (action.type) {
    case ALL_TOGGLES_REQUESTED:
      return {
        ...state,
        isFetching: true
      };

    case ALL_TOGGLES_RECEIVED:
      return {
        ...state,
        fetching: false,
        all: action.toggles
      };
    default:
      return state;
  }
};

export default reducer;
