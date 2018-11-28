const RSAA = "@@redux-api-middleware/RSAA";

export const FETCH_ALL_TOGGLES_STARTED = "FETCH_ALL_TOGGLES_STARTED";
export const FETCH_ALL_TOGGLES_FINISHED = "FETCH_ALL_TOGGLES_FINISHED";

export const fetchAllToggles = () => ({
  [RSAA]: {
    endpoint: "/api/toggles",
    method: "GET",
    types: [
      FETCH_ALL_TOGGLES_STARTED,
      FETCH_ALL_TOGGLES_FINISHED,
      "FETCH_ALL_TOGGLES_FAILURE"
    ]
  }
});

export const UPDATE_TOGGLE_NAME_STARTED = "UPDATE_TOGGLE_NAME_STARTED";
export const UPDATE_TOGGLE_NAME_FINISHED = "UPDATE_TOGGLE_NAME_FINISHED";

export const updateName = (toggleID, name) => ({
  [RSAA]: {
    endpoint: `/api/toggles/id/${toggleID}/name`,
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ name: name }),
    types: [
      { type: UPDATE_TOGGLE_NAME_STARTED, payload: { toggleID, name } },
      UPDATE_TOGGLE_NAME_FINISHED,
      "UPDATE_TOGGLE_NAME_FAILURE"
    ]
  }
});

export const UPDATE_TOGGLE_DESCRIPTION_STARTED =
  "UPDATE_TOGGLE_DESCRIPTION_STARTED";
export const UPDATE_TOGGLE_DESCRIPTION_FINISHED =
  "UPDATE_TOGGLE_DESCRIPTION_FINISHED";

export const updateDescription = (toggleID, description) => ({
  [RSAA]: {
    endpoint: `/api/toggles/id/${toggleID}/description`,
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ description: description }),
    types: [
      {
        type: UPDATE_TOGGLE_DESCRIPTION_STARTED,
        payload: { toggleID, description }
      },
      UPDATE_TOGGLE_DESCRIPTION_FINISHED,
      "UPDATE_TOGGLE_DESCRIPTION_FAILURE"
    ]
  }
});

export const CREATE_TOGGLE_STARTED = "CREATE_TOGGLE_STARTED";
export const CREATE_TOGGLE_FINISHED = "CREATE_TOGGLE_FINISHED";

export const createToggle = (name, description) => ({
  [RSAA]: {
    endpoint: `/api/toggles`,
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ name, description }),
    types: [
      {
        type: CREATE_TOGGLE_STARTED,
        payload: { name, description }
      },
      CREATE_TOGGLE_FINISHED,
      "CREATE_TOGGLE_FAILURE"
    ]
  }
});

export const CHANGE_TOGGLE_CONDITION_MODE_STARTED =
  "CHANGE_TOGGLE_CONDITION_MODE_STARTED";
export const CHANGE_TOGGLE_CONDITION_MODE_FINISHED =
  "CHANGE_TOGGLE_CONDITION_MODE_FINISHED";

export const changeConditionMode = (toggleID, conditionMode) => ({
  [RSAA]: {
    endpoint: `/api/toggles/id/${toggleID}/conditionMode`,
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ conditionMode }),
    types: [
      {
        type: CHANGE_TOGGLE_CONDITION_MODE_STARTED,
        payload: { toggleID, conditionMode }
      },
      CHANGE_TOGGLE_CONDITION_MODE_FINISHED,
      "CHANGE_TOGGLE_CONDITION_MODE_FAILURE"
    ]
  }
});

export const REMOVE_TOGGLE_TAG_STARTED = "REMOVE_TOGGLE_TAG_STARTED";
export const REMOVE_TOGGLE_TAG_FINISHED = "REMOVE_TOGGLE_TAG_FINISHED";

export const removeTag = (toggleID, tag) => ({
  [RSAA]: {
    endpoint: `/api/toggles/id/${toggleID}/tags/${tag}`,
    method: "DELETE",
    types: [
      { type: REMOVE_TOGGLE_TAG_STARTED, payload: { toggleID, tag } },
      REMOVE_TOGGLE_TAG_FINISHED,
      "REMOVE_TOGGLE_TAG_FAILURE"
    ]
  }
});

export const ADD_TOGGLE_TAG_STARTED = "ADD_TOGGLE_TAG_STARTED";
export const ADD_TOGGLE_TAG_FINISHED = "ADD_TOGGLE_TAG_FINISHED";

export const addTag = (toggleID, tag) => ({
  [RSAA]: {
    endpoint: `/api/toggles/id/${toggleID}/tags/${tag}`,
    method: "PUT",
    types: [
      { type: ADD_TOGGLE_TAG_STARTED, payload: { toggleID, tag } },
      ADD_TOGGLE_TAG_FINISHED,
      "ADD_TOGGLE_TAG_FAILURE"
    ]
  }
});
