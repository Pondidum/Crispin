import {
  ALL_TOGGLES_REQUESTED,
  ALL_TOGGLES_RECEIVED,
  UPDATE_TOGGLE_NAME_REQUESTED,
  UPDATE_TOGGLE_NAME_COMPLETED
} from "./actions";

const DefaultState = {
  isFetching: false,
  all: []
};

const reduceToggle = (state, action) => {
  switch (action.type) {
    case UPDATE_TOGGLE_NAME_REQUESTED:
      return { ...state, name: action.newName, updating: true };
    case UPDATE_TOGGLE_NAME_COMPLETED:
      return { ...state, name: action.newName, updating: false };
    default:
      return state;
  }
};

const reduceArray = (state, action) => {
  switch (action.type) {
    case UPDATE_TOGGLE_NAME_REQUESTED:
    case UPDATE_TOGGLE_NAME_COMPLETED: {
      const index = state.findIndex(t => t.id === action.toggleID);
      const newToggle = reduceToggle(state[index], action);

      return Object.assign([], state, {
        [index]: newToggle
      });
    }

    default:
      return state;
  }
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

    case UPDATE_TOGGLE_NAME_COMPLETED:
    case UPDATE_TOGGLE_NAME_REQUESTED:
      return {
        ...state,
        all: reduceArray(state.all, action)
      };

    default:
      return state;
  }
};

export default reducer;
