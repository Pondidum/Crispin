import {
  FETCH_SYSTEM_INFO_STARTED,
  FETCH_SYSTEM_INFO_FINISHED
} from "./actions";

const defaultState = {
  info: {}
};

const reducer = (state = defaultState, action) => {
  switch (action.type) {
    case FETCH_SYSTEM_INFO_STARTED:
      return {
        ...state,
        info: { ...state.info, updating: true }
      };

    case FETCH_SYSTEM_INFO_FINISHED:
      return {
        ...state,
        info: { ...action.payload, updating: false }
      };

    default:
      return state;
  }
};

export default reducer;
