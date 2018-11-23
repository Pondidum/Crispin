import {
  FETCH_ALL_TOGGLES_STARTED,
  FETCH_ALL_TOGGLES_FINISHED,
  UPDATE_TOGGLE_NAME_STARTED,
  UPDATE_TOGGLE_NAME_FINISHED,
  UPDATE_TOGGLE_DESCRIPTION_STARTED,
  UPDATE_TOGGLE_DESCRIPTION_FINISHED,
  CREATE_TOGGLE_STARTED,
  CREATE_TOGGLE_FINISHED
} from "./actions";

const DefaultState = {
  updating: false,
  all: []
};

const reduceToggle = (state, type, payload) => {
  switch (type) {
    case UPDATE_TOGGLE_NAME_STARTED:
      return { ...state, name: payload.name, updating: true };
    case UPDATE_TOGGLE_NAME_FINISHED:
      return { ...state, name: payload.name, updating: false };
    case UPDATE_TOGGLE_DESCRIPTION_STARTED:
      return { ...state, description: payload.description, updating: true };
    case UPDATE_TOGGLE_DESCRIPTION_FINISHED:
      return { ...state, description: payload.description, updating: false };
    default:
      return state;
  }
};

const reduceArray = (state, action) => {
  const index = state.findIndex(t => t.id === action.payload.toggleID);
  const newToggle = reduceToggle(state[index], action.type, action.payload);

  return Object.assign([], state, {
    [index]: newToggle
  });
};

const reducer = (state = DefaultState, action) => {
  switch (action.type) {
    case FETCH_ALL_TOGGLES_STARTED:
      return {
        ...state,
        updating: true
      };

    case FETCH_ALL_TOGGLES_FINISHED:
      return {
        ...state,
        updating: false,
        all: action.payload
      };

    case UPDATE_TOGGLE_NAME_STARTED:
    case UPDATE_TOGGLE_NAME_FINISHED:
    case UPDATE_TOGGLE_DESCRIPTION_FINISHED:
    case UPDATE_TOGGLE_DESCRIPTION_STARTED:
      return {
        ...state,
        all: reduceArray(state.all, action)
      };

    case CREATE_TOGGLE_STARTED:
      return {
        ...state,
        updating: true
      };

    case CREATE_TOGGLE_FINISHED:
      return {
        ...state,
        updating: false,
        all: [...state.all, action.payload]
      };

    default:
      return state;
  }
};

export default reducer;
