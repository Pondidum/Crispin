import {
  FETCH_ALL_TOGGLES_STARTED,
  FETCH_ALL_TOGGLES_FINISHED,
  UPDATE_TOGGLE_NAME_STARTED,
  UPDATE_TOGGLE_NAME_FINISHED,
  UPDATE_TOGGLE_DESCRIPTION_STARTED,
  UPDATE_TOGGLE_DESCRIPTION_FINISHED,
  CREATE_TOGGLE_STARTED,
  CREATE_TOGGLE_FINISHED,
  CHANGE_TOGGLE_CONDITION_MODE_STARTED,
  CHANGE_TOGGLE_CONDITION_MODE_FINISHED,
  ADD_TOGGLE_TAG_STARTED,
  ADD_TOGGLE_TAG_FINISHED,
  REMOVE_TOGGLE_TAG_STARTED,
  REMOVE_TOGGLE_TAG_FINISHED,
  ADD_TOGGLE_CONDITION_STARTED,
  ADD_TOGGLE_CONDITION_FINISHED
} from "./actions";

const DefaultState = {
  updating: false,
  all: []
};

const updating = (state, action) => {
  const isStart = action.type.endsWith("_STARTED");
  return { ...state, updating: isStart };
};

const reduceToggle = (state, type, payload) => {
  switch (type) {
    case ADD_TOGGLE_TAG_STARTED:
      return {
        ...state,
        tags: [...new Set(state.tags).add(payload.tag)]
      };

    case REMOVE_TOGGLE_TAG_STARTED:
      return {
        ...state,
        tags: state.tags.filter(t => t !== payload.tag)
      };

    case ADD_TOGGLE_CONDITION_FINISHED:
      return {
        ...state,
        conditions: [...state.conditions, payload.condition]
      };

    default:
      const { toggleID, ...delta } = payload;
      return { ...state, ...delta, id: toggleID };
  }
};

const reduceArray = (state, action) => {
  const index = state.findIndex(t => t.id === action.payload.toggleID);
  const newToggle = reduceToggle(state[index], action.type, action.payload);

  return Object.assign([], state, {
    [index]: updating(newToggle, action)
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
    case CHANGE_TOGGLE_CONDITION_MODE_STARTED:
    case CHANGE_TOGGLE_CONDITION_MODE_FINISHED:
    case ADD_TOGGLE_TAG_STARTED:
    case ADD_TOGGLE_TAG_FINISHED:
    case REMOVE_TOGGLE_TAG_STARTED:
    case REMOVE_TOGGLE_TAG_FINISHED:
    case ADD_TOGGLE_CONDITION_STARTED:
    case ADD_TOGGLE_CONDITION_FINISHED:
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
